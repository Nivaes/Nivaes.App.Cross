namespace Nivaes.App.Cross.Droid
{
    public class CrossChildViewModelOwnerAdapter : CrossBaseActivityAdapter
    {
        protected ICrossChildViewModelOwner ChildOwner => (ICrossChildViewModelOwner)Activity;

        public CrossChildViewModelOwnerAdapter(ICrossEventSourceActivity eventSource)
            : base(eventSource)
        {
            if (!(eventSource is ICrossChildViewModelOwner))
            {
                throw new CrossException("You cannot use a MvxChildViewModelOwnerAdapter on {0}",
                                       eventSource.GetType().Name);
            }
        }

        protected override void EventSourceOnDestroyCalled(object sender, EventArgs eventArgs)
        {
            ChildOwner.ClearOwnedSubIndicies();
            base.EventSourceOnDestroyCalled(sender, eventArgs);
        }

        protected override void EventSourceOnDisposeCalled(object sender, EventArgs eventArgs)
        {
            ChildOwner.ClearOwnedSubIndicies();
            base.EventSourceOnDisposeCalled(sender, eventArgs);
        }
    }
}
