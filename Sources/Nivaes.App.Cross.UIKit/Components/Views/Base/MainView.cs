namespace Nivaes.App.Cross.UIKitLib
{
    [MvxFromStoryboard("MainView")]
    public partial class MainView
        : BaseViewController<PrimaryViewModel>
    {
        public MainView()
            : base()
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ShowDetailViewController(UIViewController vc, NSObject sender)
        {
            base.ShowDetailViewController(vc, sender);
        }

        public override void ShowViewController(UIViewController vc, NSObject sender)
        {
            base.ShowViewController(vc, sender);
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //var loginService = IPlatformApplication.Current!.Services.GetRequiredService<IIdentifyService>();
            //loginService.ShowMenu();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }
        #endregion
    }
}

