using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.Components.ViewModels;
using Nivaes.App.Cross.Controls;

namespace Nivaes.App.Cross.Sample;

[RequiresUnreferencedCode("Application requires unreferenced code")]
public class SampleApp : Application, IApplication
{
    ///// <summary>
    ///// Breaking change in v6: This method is called on a background thread. Use
    ///// Startup for any UI bound actions
    ///// </summary>
    public override ICrossViewModelStar Initialize()
    {
        //CreatableTypes()
        //    .EndingWith("Service")
        //    .AsInterfaces()
        //    .RegisterAsLazySingleton();

        //var container = Singleton<CrossIoCServiceContainer>.Instance;
        //container.AddDelegate<ICrossTextProvider>(container =>
        //{
        //    return new TextProviderBuilder().TextProvider;
        //});

        //container.Merge(new ViewModelsSubcontainer());

        //RegisterAppStart<RootViewModel>();

        return new CrossViewModelStar<RootViewModel>();
    }

    ///// <summary>
    ///// Do any UI bound startup actions here
    ///// </summary>
    //public override Task Startup()
    //{
    //    return base.Startup();
    //}

    /// <summary>
    /// If the application is restarted (eg primary activity on Android
    /// can be restarted) this method will be called before Startup
    /// is called again
    /// </summary>
    //public override void Reset()
    //{
    //    base.Reset();
    //}
}
