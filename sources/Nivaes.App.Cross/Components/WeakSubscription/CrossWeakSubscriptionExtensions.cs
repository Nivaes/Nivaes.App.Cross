namespace Nivaes.App.Cross
{
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Windows.Input;
    using Nivaes.App.Cross;

    public static class CrossWeakSubscriptionExtensions
    {
        extension(INotifyPropertyChanged source)
        {
            public CrossNotifyPropertyChangedEventSubscription WeakSubscribe(EventHandler<PropertyChangedEventArgs> eventHandler)
            {
                return new CrossNotifyPropertyChangedEventSubscription(source, eventHandler);
            }

            public CrossNamedNotifyPropertyChangedEventSubscription<T> WeakSubscribe<T>(Expression<Func<T>> property,
                                                                                   EventHandler<PropertyChangedEventArgs> eventHandler)
            {
                return new CrossNamedNotifyPropertyChangedEventSubscription<T>(source, property, eventHandler);
            }

            public CrossNamedNotifyPropertyChangedEventSubscription<T> WeakSubscribe<T>(string property,
                                                                                   EventHandler<PropertyChangedEventArgs> eventHandler)
            {
                return new CrossNamedNotifyPropertyChangedEventSubscription<T>(source, property, eventHandler);
            }
        }

        extension(INotifyCollectionChanged source)
        {
            public CrossNotifyCollectionChangedEventSubscription WeakSubscribe(EventHandler<NotifyCollectionChangedEventArgs> eventHandler) => new CrossNotifyCollectionChangedEventSubscription(source, eventHandler);
        }

        extension(EventInfo eventInfo)
        {
            public CrossGeneralEventSubscription WeakSubscribe(object source,
                                                                EventHandler<EventArgs> eventHandler)
            {
                return new CrossGeneralEventSubscription(source, eventInfo, eventHandler);
            }

            public CrossValueEventSubscription<T> WeakSubscribe<T>(object source,
                                                                        EventHandler<CrossValueEventArgs<T>> eventHandler)
            {
                return new CrossValueEventSubscription<T>(source, eventInfo, eventHandler);
            }
        }

        extension(ICommand source)
        {
            public CrossCanExecuteChangedEventSubscription WeakSubscribe(EventHandler<EventArgs> eventHandler)
            {
                return new CrossCanExecuteChangedEventSubscription(source, eventHandler);
            }
        }

        extension<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] TSource>(TSource source)
            where TSource : class
        {
            public CrossWeakEventSubscription<TSource> WeakSubscribe(string eventName, EventHandler eventHandler)
            {
                return new CrossWeakEventSubscription<TSource>(source, eventName, eventHandler);
            }

            public CrossWeakEventSubscription<TSource, TEventArgs> WeakSubscribe<TEventArgs>(string eventName, EventHandler<TEventArgs> eventHandler)
            {
                return new CrossWeakEventSubscription<TSource, TEventArgs>(source, eventName, eventHandler);
            }
        }
    }
}
