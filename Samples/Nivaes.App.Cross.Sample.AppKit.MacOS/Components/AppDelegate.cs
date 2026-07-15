using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.AppKitLib;
using Nivaes.App.Cross.Hosting;
using Nivaes.App.Sample.AppKitLib;

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

    protected override void RegisterViewsActions()
    {
        base.RegisterViewsActions();
        GeneratedViewsExtensions.RegisterViewsActions();
    }
}
