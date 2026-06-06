using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.AppKitOS;
using Nivaes.App.Cross.Hosting;

namespace Nivaes.App.Cross.Sample.AppKitOS.MacOS;

[Register("AppDelegate")]
[RequiresUnreferencedCode("MvxApplicationDelegate requires unreferenced code")]
public class AppDelegate : MvxApplicationDelegate //<Setup, Nivaes.App.Cross.Sample.SampleApp>
{
    public AppDelegate()
    {
        MvxWindowPresentationAttribute.DefaultWidth = 250;
        MvxWindowPresentationAttribute.DefaultHeight = 250;
    }

    protected override CrossApp CreateCrossApp() => CrossProgram.CreateCrossApp();
}
