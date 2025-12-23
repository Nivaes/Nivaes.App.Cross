namespace Nivaes.App.Cross.UIKit.TvOS
{
    using MvvmCross.Platforms.Tvos.Views.Base;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.UIKit.TvOS;

    public class MvxViewControllerAdapter 
        : MvxBaseViewControllerAdapter
    {
        protected IMvxTvosView TvosView => ViewController as IMvxTvosView;

        public MvxViewControllerAdapter(IMvxEventSourceViewController eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxTvosView))
                throw new ArgumentException("eventSource", "eventSource should be a IMvxTvosView");
        }

        public override void HandleViewDidLoadCalled(object sender, EventArgs e)
        {
            TvosView.OnViewCreate();
            base.HandleViewDidLoadCalled(sender, e);
        }

        public override void HandleDisposeCalled(object sender, EventArgs e)
        {
            TvosView.OnViewDestroy();
            base.HandleDisposeCalled(sender, e);
        }
    }
}
