namespace Nivaes.App.Cross.Droid
{
    public class CrossBindingActivityAdapter
        : CrossBaseActivityAdapter
    {
        private ICrossAndroidBindingContext BindingContext
        {
            get
            {
                var contextOwner = (ICrossBindingContextOwner)Activity;
                return (ICrossAndroidBindingContext)contextOwner.BindingContext;
            }
        }

        public CrossBindingActivityAdapter(ICrossEventSourceActivity eventSource)
            : base(eventSource)
        {
        }

        protected override void EventSourceOnCreateWillBeCalled(object sender,
                                                                CrossValueEventArgs<Bundle> CrossValueEventArgs)
        {
            BindingContext.ClearAllBindings();
            base.EventSourceOnCreateWillBeCalled(sender, CrossValueEventArgs);
        }

        protected override void EventSourceOnDestroyCalled(object sender, EventArgs eventArgs)
        {
            BindingContext.ClearAllBindings();
            base.EventSourceOnDestroyCalled(sender, eventArgs);
        }

        protected override void EventSourceOnDisposeCalled(object sender, EventArgs eventArgs)
        {
            BindingContext.ClearAllBindings();
            base.EventSourceOnDisposeCalled(sender, eventArgs);
        }
    }
}
