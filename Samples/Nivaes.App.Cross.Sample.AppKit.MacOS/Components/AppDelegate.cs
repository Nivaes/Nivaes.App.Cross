using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.AppKitLib;
using Nivaes.App.Cross.Hosting;

namespace Nivaes.App.Cross.Sample.AppKitOS.MacOS;

[Register("AppDelegate")]
[RequiresUnreferencedCode("MvxApplicationDelegate requires unreferenced code")]
public class AppDelegate : MvxApplicationDelegate
{
    public AppDelegate()
    {
        WindowPresentationAttribute.DefaultWidth = 512;
        WindowPresentationAttribute.DefaultHeight = 512;
    }

    protected override CrossApp CreateCrossApp() => CrossProgram.CreateCrossApp(this);
}
