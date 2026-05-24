using Nivaes.App.Cross.Hosting;
using Nivaes.App.Cross.UIKitOS;

namespace Nivaes.App.Cross.Sample.UIKitOS;

[Register(nameof(AppDelegate))]
public class AppDelegate : CrossAppDelegate
{
    protected override CrossApp CreateCrossApp() => CrossProgram.CreateCrossApp();
}
