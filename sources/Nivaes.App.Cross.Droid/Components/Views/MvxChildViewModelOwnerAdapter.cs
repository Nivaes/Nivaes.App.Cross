namespace MvvmCross.Platforms.Android.Views
{
    using System;
    using MvvmCross.Platforms.Android.Views.Base;
    using Nivaes.App.Cross;

    public class MvxChildViewModelOwnerAdapter : MvxBaseActivityAdapter
    {
        protected IMvxChildViewModelOwner ChildOwner => (IMvxChildViewModelOwner)Activity;

        public MvxChildViewModelOwnerAdapter(IMvxEventSourceActivity eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxChildViewModelOwner))
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
