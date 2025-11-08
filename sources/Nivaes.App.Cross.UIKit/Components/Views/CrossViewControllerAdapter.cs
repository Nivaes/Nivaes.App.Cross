namespace Nivaes.App.Cross.UIKit
{
    public class CrossViewControllerAdapter : CrossBaseViewControllerAdapter
    {
        protected ICrossIosView? IosView => ViewController as ICrossIosView;

        public CrossViewControllerAdapter(ICrossEventSourceViewController eventSource)
            : base(eventSource)
        {
            if (eventSource is not ICrossIosView)
                throw new ArgumentException("eventSource should be a IMvxIosView", nameof(eventSource));
        }

        public override void HandleViewDidLoadCalled(object sender, EventArgs e)
        {
            IosView?.OnViewCreate();
            base.HandleViewDidLoadCalled(sender, e);
        }

        public override void HandleDisposeCalled(object sender, EventArgs e)
        {
            IosView?.OnViewDestroy();
            base.HandleDisposeCalled(sender, e);
        }
    }
}
