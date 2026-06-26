namespace Nivaes.App.Cross
{
    using System.Reflection;

    public class CrossValueEventSubscription<TEventArgs>
        : CrossWeakEventSubscription<object, CrossValueEventArgs<TEventArgs>>
    {
        public CrossValueEventSubscription(object source,
                                         EventInfo eventInfo,
                                         EventHandler<CrossValueEventArgs<TEventArgs>> eventHandler)
            : base(source, eventInfo, eventHandler)
        {
        }

        protected override Delegate CreateEventHandler()
        {
            return new EventHandler<CrossValueEventArgs<TEventArgs>>(OnSourceEvent);
        }
    }
}
