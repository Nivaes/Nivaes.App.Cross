namespace Nivaes.App.Cross.UIKit
{
    using Microsoft.Extensions.Logging;

    public static class UIViewControllerExtensions
    {
        public static ICrossIosView? GetIMvxIosView(this UIViewController? viewController)
        {
            if (viewController is ICrossIosView iosView)
            {
                return iosView;
            }

            CrossLogHost.Default?.Log(LogLevel.Warning, "Could not get IMvxIosView from ViewController {viewControllerName}",
                viewController?.GetType().Name);
            return null;
        }
    }
}
