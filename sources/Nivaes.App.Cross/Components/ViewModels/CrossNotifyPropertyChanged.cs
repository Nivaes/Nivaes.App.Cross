namespace Nivaes.App.Cross
{
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Base;
    using MvvmCross.Logging;
    using Nivaes.App.Cross;

    public abstract class CrossNotifyPropertyChanged
    : CrossMainThreadDispatchingObject, ICrossNotifyPropertyChanged
    {
        private static readonly PropertyChangedEventArgs AllPropertiesChanged = new(string.Empty);
        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangingEventHandler? PropertyChanging;

        private bool _shouldAlwaysRaiseInpcOnUserInterfaceThread;
        private bool _shouldRaisePropertyChanging;
        private bool _shouldLogInpc;

        public bool ShouldAlwaysRaiseInpcOnUserInterfaceThread()
        {
            return _shouldAlwaysRaiseInpcOnUserInterfaceThread;
        }

        public void ShouldAlwaysRaiseInpcOnUserInterfaceThread(bool value)
        {
            _shouldAlwaysRaiseInpcOnUserInterfaceThread = value;
        }

        public bool ShouldRaisePropertyChanging()
        {
            return _shouldRaisePropertyChanging;
        }

        public void ShouldRaisePropertyChanging(bool value)
        {
            _shouldRaisePropertyChanging = value;
        }
        public bool ShouldLogInpc()
        {
            return _shouldLogInpc;
        }

        public void ShouldLogInpc(bool value)
        {
            _shouldLogInpc = value;
        }

        protected CrossNotifyPropertyChanged()
        {
            var alwaysOnUIThread = CrossSingletonCache.Instance?.Settings?.AlwaysRaiseInpcOnUserInterfaceThread != false;
            ShouldAlwaysRaiseInpcOnUserInterfaceThread(alwaysOnUIThread);
            var raisePropertyChanging = CrossSingletonCache.Instance?.Settings?.ShouldRaisePropertyChanging != false;
            ShouldRaisePropertyChanging(raisePropertyChanging);
            var shouldLogInpc = CrossSingletonCache.Instance?.Settings?.ShouldLogInpc == true;
            ShouldLogInpc(shouldLogInpc);
        }

        public bool RaisePropertyChanging<T>(T newValue, Expression<Func<T>> propertyExpression)
        {
            var name = this.GetPropertyNameFromExpression(propertyExpression);
            return RaisePropertyChanging(newValue, name);
        }

        public bool RaisePropertyChanging<T>(T newValue, [CallerMemberName] string? whichProperty = "")
        {
            var changedArgs = new CrossPropertyChangingEventArgs<T>(whichProperty, newValue);
            return RaisePropertyChanging(changedArgs);
        }

        public virtual bool RaisePropertyChanging<T>(CrossPropertyChangingEventArgs<T>? changingArgs)
        {
            if (changingArgs == null)
                return false;

            // check for interception before broadcasting change
            if (InterceptRaisePropertyChanging(changingArgs)
                == CrossInpcInterceptionResult.DoNotRaisePropertyChanging)
            {
                return !changingArgs.Cancel;
            }

            if (ShouldLogInpc())
            {
                CrossLogHost.Default?.Log(LogLevel.Trace, "Property '{PropertyName}' changing value to {NewValue}",
                    changingArgs.PropertyName, changingArgs.NewValue);
            }

            PropertyChanging?.Invoke(this, changingArgs);

            return !changingArgs.Cancel;
        }

#pragma warning disable CA1030 // Use events where appropriate
        public virtual Task RaiseAllPropertiesChanged()
#pragma warning restore CA1030 // Use events where appropriate
        {
            return RaisePropertyChanged(AllPropertiesChanged);
        }

        public Task RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var name = this.GetPropertyNameFromExpression(propertyExpression);
            return RaisePropertyChanged(name);
        }

        public virtual Task RaisePropertyChanged([CallerMemberName] string? whichProperty = "")
        {
            var changedArgs = new PropertyChangedEventArgs(whichProperty);
            return RaisePropertyChanged(changedArgs);
        }

        public virtual async Task RaisePropertyChanged(PropertyChangedEventArgs changedArgs)
        {
            // check for interception before broadcasting change
            if (InterceptRaisePropertyChanged(changedArgs)
                == CrossInpcInterceptionResult.DoNotRaisePropertyChanged)
            {
                return;
            }

            void RaiseChange()
            {
                if (ShouldLogInpc())
                    CrossLogHost.Default?.Log(LogLevel.Trace, "Property '{PropertyName}' value changed", changedArgs.PropertyName);
                PropertyChanged?.Invoke(this, changedArgs);
            }

            void ExceptionMasked() => CrossMainThreadDispatcher.ExceptionMaskedAction(RaiseChange, true);

            if (ShouldAlwaysRaiseInpcOnUserInterfaceThread())
            {
                // check for subscription before potentially causing a cross-threaded call
                if (PropertyChanged == null)
                    return;

                await InvokeOnMainThreadAsync(ExceptionMasked).ConfigureAwait(true);
            }
            else
            {
                ExceptionMasked();
            }
        }

        protected virtual void SetProperty<T>(ref T storage, T value, Action<bool>? action, [CallerMemberName] string? propertyName = null)
        {
            if (action == null)
            {
                throw new ArgumentException($"{nameof(action)} should not be null", nameof(action));
            }

            action.Invoke(SetProperty(ref storage, value, propertyName));
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, Action? afterAction, [CallerMemberName] string? propertyName = null)
        {
            if (SetProperty(ref storage, value, propertyName))
            {
                afterAction?.Invoke();
                return true;
            }

            return false;
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            if (ShouldRaisePropertyChanging())
            {
                var shouldSetValue = RaisePropertyChanging(value, propertyName);
                if (!shouldSetValue)
                    return false;
            }

            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        protected virtual CrossInpcInterceptionResult InterceptRaisePropertyChanged(PropertyChangedEventArgs changedArgs)
        {
            if (CrossSingletonCache.Instance != null)
            {
                var interceptor = CrossSingletonCache.Instance.InpcInterceptor;
                if (interceptor != null)
                {
                    return interceptor.Intercept(this, changedArgs);
                }
            }

            return CrossInpcInterceptionResult.NotIntercepted;
        }

        protected virtual CrossInpcInterceptionResult InterceptRaisePropertyChanging(PropertyChangingEventArgs changingArgs)
        {
            if (CrossSingletonCache.Instance != null)
            {
                var interceptor = CrossSingletonCache.Instance.InpcInterceptor;
                if (interceptor != null)
                {
                    return interceptor.Intercept(this, changingArgs);
                }
            }

            return CrossInpcInterceptionResult.NotIntercepted;
        }
    }
}