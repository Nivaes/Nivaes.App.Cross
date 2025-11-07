namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Specialized;
    using System.Reflection;

    [Obsolete("No es compatible con AoT", true)]
    public class CrossNotifyCollectionChangedEventSubscription
        : CrossWeakEventSubscription<INotifyCollectionChanged, NotifyCollectionChangedEventArgs>
    {
        private static readonly EventInfo EventInfo = typeof(INotifyCollectionChanged).GetEvent("CollectionChanged");

        // This code ensures the CollectionChanged event is not stripped by Xamarin linker
        // see https://github.com/MvvmCross/MvvmCross/pull/453
        public static void LinkerPleaseInclude(INotifyCollectionChanged iNotifyCollectionChanged)
        {
            iNotifyCollectionChanged.CollectionChanged += (sender, e) => { };
        }

        public CrossNotifyCollectionChangedEventSubscription(INotifyCollectionChanged source,
                                                           EventHandler<NotifyCollectionChangedEventArgs> targetEventHandler)
            : base(source, EventInfo, targetEventHandler)
        {
        }

        protected override Delegate CreateEventHandler()
        {
            return new NotifyCollectionChangedEventHandler(OnSourceEvent);
        }
    }
#nullable restore
}
