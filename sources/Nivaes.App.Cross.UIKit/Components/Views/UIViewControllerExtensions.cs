namespace Nivaes.App.Cross.UIKit
{
    using Microsoft.Extensions.Logging;

    public static class UIViewControllerExtensions
    {
        extension(UIViewController? viewController)
        {
            public IMvxIosView? GetIMvxIosView()
            {
                if (viewController is IMvxIosView iosView)
                {
                    return iosView;
                }

                CrossLogHost.Default?.Log(LogLevel.Warning, "Could not get IMvxIosView from ViewController {viewControllerName}",
                    viewController?.GetType().Name);
                return null;
            }
        }
    }
}
