namespace Nivaes.App.Cross
{
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Windows.Input;

    public static class CrossWeakSubscriptionExtensions
    {
        public static CrossNotifyPropertyChangedEventSubscription WeakSubscribe(this INotifyPropertyChanged source,
                                                                              EventHandler<PropertyChangedEventArgs> eventHandler)
        {
            return new CrossNotifyPropertyChangedEventSubscription(source, eventHandler);
        }

        public static CrossNamedNotifyPropertyChangedEventSubscription<T> WeakSubscribe<T>(this INotifyPropertyChanged source,
                                                                               Expression<Func<T>> property,
                                                                               EventHandler<PropertyChangedEventArgs> eventHandler)
        {
            return new CrossNamedNotifyPropertyChangedEventSubscription<T>(source, property, eventHandler);
        }

        public static CrossNamedNotifyPropertyChangedEventSubscription<T> WeakSubscribe<T>(this INotifyPropertyChanged source,
                                                                               string property,
                                                                               EventHandler<PropertyChangedEventArgs> eventHandler)
        {
            return new CrossNamedNotifyPropertyChangedEventSubscription<T>(source, property, eventHandler);
        }

        [Obsolete("No compatible con AoT", true)]
        public static CrossNotifyCollectionChangedEventSubscription WeakSubscribe(this INotifyCollectionChanged source,
                                                                                EventHandler<NotifyCollectionChangedEventArgs> eventHandler)
        {
            return new CrossNotifyCollectionChangedEventSubscription(source, eventHandler);
        }

        public static CrossGeneralEventSubscription WeakSubscribe(this EventInfo eventInfo,
                                                                object source,
                                                                EventHandler<EventArgs> eventHandler)
        {
            return new CrossGeneralEventSubscription(source, eventInfo, eventHandler);
        }

        [Obsolete("No compatible con AoT", true)]
        public static CrossValueEventSubscription<T> WeakSubscribe<T>(this EventInfo eventInfo,
                                                                    object source,
                                                                    EventHandler<CrossValueEventArgs<T>> eventHandler)
        {
            return new CrossValueEventSubscription<T>(source, eventInfo, eventHandler);
        }

        public static CrossCanExecuteChangedEventSubscription WeakSubscribe(this ICommand source,
                                                                          EventHandler<EventArgs> eventHandler)
        {
            return new CrossCanExecuteChangedEventSubscription(source, eventHandler);
        }

        public static CrossWeakEventSubscription<TSource> WeakSubscribe<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] TSource>(
            this TSource source, string eventName, EventHandler eventHandler)
                where TSource : class
        {
            return new CrossWeakEventSubscription<TSource>(source, eventName, eventHandler);
        }

        public static CrossWeakEventSubscription<TSource, TEventArgs> WeakSubscribe<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] TSource, TEventArgs>(
            this TSource source, string eventName, EventHandler<TEventArgs> eventHandler)
                where TSource : class
        {
            return new CrossWeakEventSubscription<TSource, TEventArgs>(source, eventName, eventHandler);
        }
    }
}

