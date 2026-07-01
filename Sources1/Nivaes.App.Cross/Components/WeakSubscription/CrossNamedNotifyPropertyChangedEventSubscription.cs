namespace Nivaes.App.Cross
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;

    public class CrossNamedNotifyPropertyChangedEventSubscription<T>
        : CrossNotifyPropertyChangedEventSubscription
    {
        private readonly string _propertyName;

        public CrossNamedNotifyPropertyChangedEventSubscription(INotifyPropertyChanged source,
                                                              Expression<Func<T>> property,
                                                              EventHandler<PropertyChangedEventArgs> targetEventHandler)
            : this(source, source.GetPropertyNameFromExpression(property), targetEventHandler)
        {
        }

        public CrossNamedNotifyPropertyChangedEventSubscription(INotifyPropertyChanged source,
                                                              string propertyName,
                                                              EventHandler<PropertyChangedEventArgs> targetEventHandler)
            : base(source, targetEventHandler)
        {
            _propertyName = propertyName;
        }

        protected override Delegate CreateEventHandler()
        {
            return new PropertyChangedEventHandler((sender, e) =>
                {
                    if (string.IsNullOrEmpty(e.PropertyName)
                        || e.PropertyName == _propertyName)
                    {
                        OnSourceEvent(sender, e);
                    }
                });
        }
    }
}
