namespace MvvmCross.Platforms.Tvos.Views
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.Logging;
    using UIKit;

    public static class UIViewControllerExtensions
    {
        public static IMvxTvosView GetIMvxTvosView(this UIViewController viewController)
        {
            var mvxView = viewController as IMvxTvosView;
            if (mvxView == null)
            {
                MvxLogHost.Default?.Log(
                    LogLevel.Warning, "Could not get IMvxIosView from ViewController {viewControllerName}", viewController.GetType().Name);
            }
            return mvxView;
        }
    }
}
