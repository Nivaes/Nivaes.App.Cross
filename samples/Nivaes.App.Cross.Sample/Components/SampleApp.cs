using System.Diagnostics.CodeAnalysis;
using Nivaes.IoC;

namespace Nivaes.App.Cross.Sample;

[RequiresUnreferencedCode("MvxApplication requires unreferenced code")]
public class SampleApp : CrossApplication
{
    /// <summary>
    /// Breaking change in v6: This method is called on a background thread. Use
    /// Startup for any UI bound actions
    /// </summary>
    public override void Initialize()
    {
        //CreatableTypes()
        //    .EndingWith("Service")
        //    .AsInterfaces()
        //    .RegisterAsLazySingleton();

        var container = Singleton<CrossIoCServiceContainer>.Instance;
        container.AddDelegate<ICrossTextProvider>(container =>
        {
            return new TextProviderBuilder().TextProvider;
        });

        container.Merge(new ViewModelsSubcontainer());

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
