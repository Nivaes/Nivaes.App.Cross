namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    public class CrossGeneralEventSubscription(
            object source,
            EventInfo eventInfo,
            EventHandler<EventArgs> eventHandler)
        : CrossWeakEventSubscription<object, EventArgs>(source, eventInfo, eventHandler)
    {
        protected override Delegate CreateEventHandler() => new EventHandler(OnSourceEvent);
    }

    public class MvxGeneralEventSubscription<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] TSource, TEventArgs>
        : CrossWeakEventSubscription<TSource, TEventArgs>
        where TSource : class
        where TEventArgs : EventArgs
    {
        public MvxGeneralEventSubscription(
                TSource source,
                EventInfo eventInfo,
                EventHandler<TEventArgs> eventHandler)
            : base(source, eventInfo, eventHandler)
        {
        }

        public MvxGeneralEventSubscription(
                TSource source,
                string eventName,
                EventHandler<TEventArgs> eventHandler)
            : base(source, typeof(TSource).GetEvent(eventName)!, eventHandler)
        {
        }

        protected override Delegate CreateEventHandler() => new EventHandler<TEventArgs>(OnSourceEvent);
    }
}