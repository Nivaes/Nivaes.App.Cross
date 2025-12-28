using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross.Sample;

[RequiresUnreferencedCode("MvxApplication requires unreferenced code")]
public class App : CrossApplication
{
    /// <summary>
    /// Breaking change in v6: This method is called on a background thread. Use
    /// Startup for any UI bound actions
    /// </summary>
    public override void Initialize()
    {
        throw new NotImplementedException();

        //CreatableTypes()
        //    .EndingWith("Service")
        //    .AsInterfaces()
        //    .RegisterAsLazySingleton();

        //Mvx.IoCProvider?.RegisterSingleton<ICrossTextProvider>(new TextProviderBuilder().TextProvider);

        RegisterAppStart<RootViewModel>();
    }

    /// <summary>
    /// Do any UI bound startup actions here
    /// </summary>
    public override Task Startup()
    {
        return base.Startup();
    }

    /// <summary>
    /// If the application is restarted (eg primary activity on Android
    /// can be restarted) this method will be called before Startup
    /// is called again
    /// </summary>
    public override void Reset()
    {
        base.Reset();
    }
}
