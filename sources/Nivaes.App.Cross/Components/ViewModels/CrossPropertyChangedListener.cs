namespace Nivaes.App.Cross
{
    using System.ComponentModel;
    using System.Linq.Expressions;
    using MvvmCross.Base;

    public class CrossPropertyChangedListener
        : IDisposable
    {
        private readonly Dictionary<string, List<PropertyChangedEventHandler>> _handlersLookup = new();

        private readonly INotifyPropertyChanged _notificationObject;
        private readonly CrossNotifyPropertyChangedEventSubscription _token;

        public CrossPropertyChangedListener(INotifyPropertyChanged notificationObject)
        {
            ArgumentNullException.ThrowIfNull(notificationObject);

            _notificationObject = notificationObject;
            _token = _notificationObject.WeakSubscribe(NotificationObjectOnPropertyChanged);
        }

        // Note - this is public because we use it in weak referenced situations
        public virtual void NotificationObjectOnPropertyChanged(object? sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var whichProperty = propertyChangedEventArgs.PropertyName;

            List<PropertyChangedEventHandler>? handlers;
            if (string.IsNullOrEmpty(whichProperty))
            {
                // if whichProperty is empty, then it means everything has changed
                handlers = _handlersLookup.Values.SelectMany(x => x).ToList();
            }
            else
            {
                if (!_handlersLookup.TryGetValue(whichProperty, out handlers))
                    return;
            }

            foreach (var propertyChangedEventHandler in handlers)
            {
                propertyChangedEventHandler(sender, propertyChangedEventArgs);
            }
        }

        ~CrossPropertyChangedListener()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _token.Dispose();
                Clear();
            }
        }

        public void Clear()
        {
            _handlersLookup.Clear();
        }

        public CrossPropertyChangedListener Listen<TProperty>(Expression<Func<TProperty>> property, Action handler)
        {
            return Listen(property, (_, _) => handler());
        }

        public CrossPropertyChangedListener Listen<TProperty>(
            Expression<Func<TProperty>> propertyExpression, PropertyChangedEventHandler handler)
        {
            var propertyName = _notificationObject.GetPropertyNameFromExpression(propertyExpression);
            return Listen(propertyName, handler);
        }

        public CrossPropertyChangedListener Listen(string propertyName, Action handler)
        {
            return Listen(propertyName, (s, e) => handler());
        }

        public CrossPropertyChangedListener Listen(string propertyName, PropertyChangedEventHandler handler)
        {
            if (!_handlersLookup.TryGetValue(propertyName, out List<PropertyChangedEventHandler>? handlers))
            {
                handlers = new List<PropertyChangedEventHandler>();
                _handlersLookup.Add(propertyName, handlers);
            }

            handlers.Add(handler);
            return this;
        }
    }
}