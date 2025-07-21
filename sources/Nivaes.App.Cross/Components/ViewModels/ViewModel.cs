namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public abstract class ViewModel
        : IViewModel, INotifyPropertyChanged
    {
        public string Title { get; set; } = "Root View";

        #region IViewModel
        public virtual void ViewCreated()
        {
        }

        public virtual void ViewAppearing()
        {
        }

        public virtual void ViewAppeared()
        {
        }

        public virtual void ViewDisappearing()
        {
        }

        public virtual void ViewDisappeared()
        {
        }

        public virtual void ViewDestroy(bool viewFinishing = true)
        {
        }

        public void Init(IBundle parameters)
        {
            InitFromBundle(parameters);
        }

        public void ReloadState(IBundle state)
        {
            ReloadFromBundle(state);
        }

        public virtual void Start()
        {
        }

        public void SaveState(IBundle state)
        {
            SaveStateToBundle(state);
        }

        protected virtual void InitFromBundle(IBundle parameters)
        {
        }

        protected virtual void ReloadFromBundle(IBundle state)
        {
        }

        protected virtual void SaveStateToBundle(IBundle bundle)
        {
        }

        public virtual void Prepare()
        {
        }

        public virtual Task Initialize()
        {
            return Task.FromResult(true);
        }

        //private NotifyTask _initializeTask;
        //public NotifyTask InitializeTask
        //{
        //    get => _initializeTask;
        //    set => SetProperty(ref _initializeTask, value);
        //}
        #endregion

        #region INotifyPropertyChanged
        /// <summary>Occurs when a property value changes.</summary>
        private PropertyChangedEventHandler? mPropertyChanged;

        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler? PropertyChanged
        {
            add => mPropertyChanged += value;
            remove => mPropertyChanged -= value;
        }

        /// <summary>Raises the <see cref="PropertyChanged"/> event.</summary>
        /// <param name="propertyName">The property name of the property that has changed.</param>
        //[DebuggerStepThrough]
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>Raises the <see cref="PropertyChanged"/> event.</summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        //[DebuggerStepThrough]
        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            mPropertyChanged?.Invoke(this, e);
        }

        /// <summary>Change value of property.</summary>
        /// <exception cref="ArgumentNullException"></exception>
        //[DebuggerStepThrough]
        protected void RegisterNewValueProperty<T>(T property)
            where T : IModel
        {
            if (object.Equals(property, default(T))) throw new ArgumentNullException(nameof(property));

            property.PropertyChanged += RaisePropertyChanged;
        }

        /// <summary>Unregister change value of property.</summary>
        /// <exception cref="ArgumentNullException"></exception>
        //[DebuggerStepThrough]
        protected void UnregisterNewValueProperty<T>(T property)
            where T : IModel
        {
            if (object.Equals(property, default(T))) throw new ArgumentNullException(nameof(property));

            property.PropertyChanged -= RaisePropertyChanged;
        }

        /// <summary>Change value of property.</summary>
        //[DebuggerStepThrough]
        protected bool SetProperty<T>(ref T property, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (object.Equals((object?)property, (object?)newValue))
            {
                return false;
            }
            else
            {
                IModel? propertyModel = property as IModel;

                if (propertyModel != null)
                    propertyModel.PropertyChanged -= RaisePropertyChanged;

                property = newValue;

                if (propertyModel != null)
                    propertyModel.PropertyChanged += RaisePropertyChanged;

                RaisePropertyChanged(propertyName);

                return true;
            }
        }

        //[DebuggerStepThrough]
        protected bool SetProperty<T>(ref T property, T newValue,
            Action<NotifyCollectionChangedEventArgs> notificationCollectionChanged, [CallerMemberName] string propertyName = "")
            where T : INotifyCollectionChanged
        {
            if (object.Equals((object)property, (object)newValue))
            {
                return false;
            }
            else
            {
                IModel? propertyModel = property as IModel;

                if (propertyModel != null)
                    propertyModel.PropertyChanged -= RaisePropertyChanged;

                property = newValue;

                if (propertyModel != null)
                    propertyModel.PropertyChanged += RaisePropertyChanged;

                if (!object.Equals(property, default(T)))
                    property.CollectionChanged += (o, e) => notificationCollectionChanged(e);

                RaisePropertyChanged(propertyName);

                return true;
            }
        }

        /// <summary>Response to <see cref="INotifyPropertyChanged.PropertyChanged"/> of fiscal office.</summary>
        //[DebuggerStepThrough]
        private void RaisePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            mPropertyChanged?.Invoke(sender, e);
        }
        #endregion
    }
}
