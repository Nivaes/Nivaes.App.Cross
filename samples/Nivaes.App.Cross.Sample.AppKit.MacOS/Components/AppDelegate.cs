using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.AppKitOS;
using Nivaes.App.Cross.Hosting;

namespace Nivaes.App.Cross.Sample.AppKitOS.MacOS;

[Register("AppDelegate")]
[RequiresUnreferencedCode("MvxApplicationDelegate requires unreferenced code")]
#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
public class AppDelegate : MvxApplicationDelegate //<Setup, Nivaes.App.Cross.Sample.SampleApp>
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
{
    public AppDelegate()
    {
        MvxWindowPresentationAttribute.DefaultWidth = 250;
        MvxWindowPresentationAttribute.DefaultHeight = 250;
    }

    protected override CrossApp CreateCrossApp() => CrossProgram.CreateCrossApp();
}
