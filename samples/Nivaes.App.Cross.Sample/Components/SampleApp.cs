using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Components.ViewModels;
using Nivaes.App.Cross.Controls;
using OpenTelemetry.Trace;

namespace Nivaes.App.Cross.Sample;

[RequiresUnreferencedCode("Application requires unreferenced code")]
public class SampleApp : Application, IApplication
{
    private readonly TracerProvider _tracerProvider;

    //public SampleApp(ILogger<SampleApp> logger)
    //    : base(logger)
    //{ }

    public SampleApp(ILogger<SampleApp> logger, TracerProvider tracerProvider)
        : base(logger)
    {
        _tracerProvider = tracerProvider;
    }

    ///// <summary>
    ///// Breaking change in v6: This method is called on a background thread. Use
    ///// Startup for any UI bound actions
    ///// </summary>
    public override ICrossViewModelStar Initialize()
    {
        using (Logger.BeginScope("Initialice app"))
        {
            var source = new ActivitySource("SampleCrossClient");

            using (var activity = source.StartActivity("SampleCrossClient"))
            {
                activity?.SetTag("test", "true");
            }

            //_tracerProvider.ForceFlush();

            //var logger2 = _services.GetRequiredService<ILoggerFactory>().CreateLogger("Test");
            //logger2.LogInformation("Hola OpenTelemetry");

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
