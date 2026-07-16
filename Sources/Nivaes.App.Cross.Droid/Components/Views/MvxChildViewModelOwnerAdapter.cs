namespace Nivaes.App.Cross.Droid
{
    using System;

    public class MvxChildViewModelOwnerAdapter
        : MvxBaseActivityAdapter
    {
        protected IMvxChildViewModelOwner? ChildOwner => (IMvxChildViewModelOwner?)Activity;

        public MvxChildViewModelOwnerAdapter(ICrossEventSourceActivity eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxChildViewModelOwner))
            {
                throw new AppException("You cannot use a MvxChildViewModelOwnerAdapter on {0}",
                                       eventSource.GetType().Name);
            }
        }

        protected override void EventSourceOnDestroyCalled(object? sender, EventArgs eventArgs)
        {
            ChildOwner?.ClearOwnedSubIndicies();
            base.EventSourceOnDestroyCalled(sender, eventArgs);
        }

        protected override void EventSourceOnDisposeCalled(object? sender, EventArgs eventArgs)
        {
            ChildOwner?.ClearOwnedSubIndicies();
            base.EventSourceOnDisposeCalled(sender, eventArgs);
        }
    }
}
