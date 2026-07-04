using Nivaes.App.Cross.UIKitLib;

namespace Nivaes.App.Cross.Sample.UIKitLib;

[Register(nameof(AppDelegate))]
public class AppDelegate : CrossAppDelegate
{
    public override bool WillFinishLaunching(UIApplication application, NSDictionary? launchOptions)
    {
        return true;
    }
}
