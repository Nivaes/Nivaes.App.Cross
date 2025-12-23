namespace Nivaes.App.Cross.UIKit
{
    public class MvxViewControllerAdapter
        : MvxBaseViewControllerAdapter
    {
        protected IMvxIosView? IosView => ViewController as IMvxIosView;

        public MvxViewControllerAdapter(IMvxEventSourceViewController eventSource)
            : base(eventSource)
        {
            if (eventSource is not IMvxIosView)
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
