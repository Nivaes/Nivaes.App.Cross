namespace Nivaes.App.Cross.UIKitOS
{
    public class MvxViewControllerAdapter
        : MvxBaseViewControllerAdapter
    {
        protected IMvxIosView? IosView => ViewController as IMvxIosView;

        public MvxViewControllerAdapter(IMvxEventSourceViewController eventSource)
            : base(eventSource)
        {

            if (eventSource is not IMvxIosView)
                throw new ArgumentException($"eventSource should be a {nameof(IMvxIosView)}");
        }

        public override void HandleViewDidLoadCalled(object? sender, EventArgs e)
        {
            IosView?.OnViewCreate();
            base.HandleViewDidLoadCalled(sender, e);
        }

        public override void HandleDisposeCalled(object? sender, EventArgs e)
        {
            IosView?.OnViewDestroy();
            base.HandleDisposeCalled(sender, e);
        }
    }
}
