// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Autofac;
    using MvvmCross.Annotations;
    using MvvmCross.Base;
    using Nivaes.App.Cross.IoC;
    using Nivaes.App.Cross.Logging;

    public abstract class MvxNotifyPropertyChanged
        : MvxMainThreadDispatchingObject
        , IMvxNotifyPropertyChanged
    {
        private static readonly PropertyChangedEventArgs AllPropertiesChanged = new PropertyChangedEventArgs(string.Empty);
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

        protected MvxNotifyPropertyChanged()
        {
            var settings = IoCContainer.ComponentContext.Resolve<IMvxSettings>();

            var alwaysOnUIThread = settings.AlwaysRaiseInpcOnUserInterfaceThread;
            ShouldAlwaysRaiseInpcOnUserInterfaceThread(alwaysOnUIThread);

            var raisePropertyChanging = settings.ShouldRaisePropertyChanging;
            ShouldRaisePropertyChanging(raisePropertyChanging);

            var shouldLogInpc = settings.ShouldLogInpc;
            ShouldLogInpc(shouldLogInpc);
        }

        public bool RaisePropertyChanging<T>(T newValue, Expression<Func<T>> property)
        {
            var name = this.GetPropertyNameFromExpression(property);
            return RaisePropertyChanging(newValue, name);
        }

        public bool RaisePropertyChanging<T>(T newValue, [CallerMemberName] string whichProperty = "")
        {
            var changedArgs = new MvxPropertyChangingEventArgs<T>(whichProperty, newValue);
            return RaisePropertyChanging(changedArgs);
        }

        public virtual bool RaisePropertyChanging<T>(MvxPropertyChangingEventArgs<T> changingArgs)
        {
            if (changingArgs == null) throw new ArgumentNullException(nameof(changingArgs));

            if (ShouldLogInpc())
                MvxLog.Instance?.Trace($"Property '{changingArgs.PropertyName}' changing value to {changingArgs.NewValue}");

            PropertyChanging?.Invoke(this, changingArgs);

            return !changingArgs.Cancel;
        }

        [NotifyPropertyChangedInvocator]
        public Task RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            var name = this.GetPropertyNameFromExpression(property);
            return RaisePropertyChanged(name);
        }

        [NotifyPropertyChangedInvocator]
        public virtual Task RaisePropertyChanged([CallerMemberName] string whichProperty = "")
        {
            var changedArgs = new PropertyChangedEventArgs(whichProperty);
            return RaisePropertyChanged(changedArgs);
        }

        public virtual Task RaiseAllPropertiesChanged()
        {
            return RaisePropertyChanged(AllPropertiesChanged);
        }

        public virtual async Task RaisePropertyChanged(PropertyChangedEventArgs changedArgs)
        {
            ValueTask<bool> raiseChange()
            {
                if (ShouldLogInpc())
                    MvxLog.Instance?.Trace($"Property '{changedArgs.PropertyName}' value changed");
                PropertyChanged?.Invoke(this, changedArgs);

                return new ValueTask<bool>(true);
            }

            ValueTask<bool> exceptionMasked() => MvxMainThreadDispatcher.ExceptionMaskedActionAsync(raiseChange, true);

            if (ShouldAlwaysRaiseInpcOnUserInterfaceThread())
            {
                // check for subscription before potentially causing a cross-threaded call
                if (PropertyChanged == null)
                    return;

                await InvokeOnMainThreadAsync(exceptionMasked).ConfigureAwait(false);
            }
            else
            {
                await exceptionMasked().ConfigureAwait(false);
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void SetProperty<T>(ref T storage, T value, Action<bool> action, [CallerMemberName] string propertyName = "")
        {
            if (action == null) throw new ArgumentNullException(nameof(action), $"{nameof(action)} should not be null");

            action.Invoke(SetProperty(ref storage, value, propertyName));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual bool SetProperty<T>(ref T storage, T value, Action afterAction, [CallerMemberName] string propertyName = "")
        {
            if (SetProperty(ref storage, value, propertyName))
            {
                afterAction?.Invoke();
                return true;
            }

            return false;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
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
    }
}
