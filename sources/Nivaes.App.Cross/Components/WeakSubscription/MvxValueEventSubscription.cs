namespace MvvmCross.WeakSubscription
{
    using System.Reflection;
    using Nivaes.App.Cross;

    public class MvxValueEventSubscription<TEventArgs>
        : MvxWeakEventSubscription<object, CrossValueEventArgs<TEventArgs>>
    {
        public MvxValueEventSubscription(object source,
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
