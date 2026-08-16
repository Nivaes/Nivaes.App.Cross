using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;

namespace Nivaes.App.Cross
{
    /// <summary>Controller for event listener.</summary>
    public sealed class DataModelPropertyChangedController : IDisposable
    {
        #region Properties
        private List<IWeakEventListener>? _weakEventListeners;

        private readonly object _rootSource;

        private readonly ExPropertyChangedEventHandler _propertyChangedEventHandler;

        private readonly ExNotifyCollectionChangedEventHandler? _notifyCollectionChangedEventArgs;
        #endregion

        #region Constructor
        /// <summary>Create a new instance of <see cref="DataModelPropertyChangedController"/>.</summary>
        public DataModelPropertyChangedController(object source, ExPropertyChangedEventHandler propertyChangedEventHandler)
        {
            _propertyChangedEventHandler = propertyChangedEventHandler ?? throw new ArgumentNullException(nameof(propertyChangedEventHandler));
            _rootSource = source ?? throw new ArgumentNullException(nameof(source));

            _weakEventListeners = new List<IWeakEventListener>();

            var elements = ListTree(source);

            foreach (ModelElementInformation element in elements)
            {
                AddWeakEventListenerElement(element);
            }
        }

        /// <summary>Create a new instance of <see cref="DataModelPropertyChangedController"/>.</summary>
        public DataModelPropertyChangedController(object source,
                ExPropertyChangedEventHandler propertyChangedEventHandler, ExNotifyCollectionChangedEventHandler notifyCollectionChangedEventHandler)
            : this(source, propertyChangedEventHandler)
        {
            _notifyCollectionChangedEventArgs = notifyCollectionChangedEventHandler ?? throw new ArgumentNullException(nameof(notifyCollectionChangedEventHandler));
        }
        #endregion

        #region Weak tree listener
        /// <summary>Adds a weak event listener element.</summary>
        private void AddWeakEventListenerElement(ModelElementInformation element)
        {
            if (element.IsCollection)
            {
                if (element.Element is INotifyCollectionChanged notifyCollectionChanged)
                {
                    string path = element.Path;
                    WeakEventListener<INotifyCollectionChanged, object, NotifyCollectionChangedEventArgs> weak =
                        new WeakEventListener<INotifyCollectionChanged, object, NotifyCollectionChangedEventArgs>(notifyCollectionChanged)
                        {
                            OnEventAction = (instance, source, eventArgs) =>
                            {
                                _notifyCollectionChangedEventArgs?.Invoke(source, new ExNotifyCollectionChangedEventArgs(source, _rootSource, path, eventArgs.Action, eventArgs.NewItems!, eventArgs.OldItems!));
                            },
                            OnDetachAction = (listener) =>
                            {

                            }
                        };

                    notifyCollectionChanged.CollectionChanged += weak.OnEvent;

                    _weakEventListeners?.Add(weak);
                }
            }
            else
            {
                if (element.Element is INotifyPropertyChanged notifyPropertyChanged)
                {
                    string path = element.Path;
                    WeakEventListener<INotifyPropertyChanged, object, PropertyChangedEventArgs> weak =
                        new WeakEventListener<INotifyPropertyChanged, object, PropertyChangedEventArgs>(notifyPropertyChanged)
                        {
                            OnEventAction = (instance, source, eventArgs) =>
                            {
                                _propertyChangedEventHandler?.Invoke(source, new ExPropertyChangedEventArgs(eventArgs.PropertyName!, source!, _rootSource, path));
                            },
                            OnDetachAction = (listener) =>
                            {

                            }
                        };

                    notifyPropertyChanged.PropertyChanged += weak.OnEvent;

                    _weakEventListeners?.Add(weak);
                }
            }
        }
        #endregion

        #region Tree
        /// <summary>List a object of a tree.</summary>
        private IEnumerable<ModelElementInformation> ListTree(object source, string path = "")
        {
            yield return new ModelElementInformation(source, path, false);

            Type type = source.GetType();

            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.CanRead && typeof(INotifyPropertyChanged).IsAssignableFrom(property.PropertyType)
                    && !typeof(ICommand).IsAssignableFrom(property.PropertyType)
                    && property.GetCustomAttributes(typeof(IgnoreEventListenerAttribute), true).Length == 0)
                {
                    foreach (ModelElementInformation elementInformation in AddNotifyPropertyListTree(property, source, path))
                        yield return elementInformation;
                }

                if (source is INotifyCollectionChanged)
                {
                    yield return new ModelElementInformation(source, path, true);
                }
            }
        }

        /// <summary>Add a <see cref="INotifyPropertyChanged"/> in list tree.</summary>
        private IEnumerable<ModelElementInformation> AddNotifyPropertyListTree(PropertyInfo property, object source, string path)
        {
            ParameterInfo[] parameters = property.GetIndexParameters();

            if (parameters.Length == 0)
            {
                if (property.GetValue(source, null) is INotifyPropertyChanged objectSource)
                {
                    string newPath = path + property.Name + ".";
                    foreach (ModelElementInformation element in ListTree(objectSource, newPath))
                        yield return element;
                }
            }
            else if (parameters.Length == 1 && parameters[0].ParameterType == typeof(int))
            {
                int i = 0;
                string newPath = path + "[].";
                while (true)
                {
                    object? objectSource;
                    try
                    {
                        objectSource = property.GetValue(source, new object[] { ++i });
                    }
                    catch (TargetInvocationException) { break; }

                    if (objectSource != null)
                    {
                        foreach (ModelElementInformation element in ListTree(objectSource, newPath))
                            yield return element;
                    }
                }
            }
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_weakEventListeners != null)
                {
                    foreach (var weakEventListener in _weakEventListeners)
                    {
                        weakEventListener.Detach();
                    }

                    _weakEventListeners = null;
                }
            }
        }
        #endregion
    }
}
