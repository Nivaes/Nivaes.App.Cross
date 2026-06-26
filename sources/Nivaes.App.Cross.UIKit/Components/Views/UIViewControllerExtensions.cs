using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;

namespace Nivaes.App.Cross.UIKitOS
{
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

                CrossLoggerHost.GetLogger(nameof(UIViewControllerExtensions)).Log(LogLevel.Warning, "Could not get IMvxIosView from ViewController {viewControllerName}",
                    viewController?.GetType().Name);
                return null;
            }
        }
    }
}
