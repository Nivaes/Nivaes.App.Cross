using Nivaes.App.Cross.UIKitOS;

namespace Nivaes.App.Cross.Sample.UIKitOS;

[Register(nameof(AppDelegate))]
public class AppDelegate : CrossAppDelegate
{
    public override bool WillFinishLaunching(UIApplication application, NSDictionary? launchOptions)
    {
        return true;
    }
}
