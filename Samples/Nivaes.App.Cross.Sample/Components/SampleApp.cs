using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Components.ViewModels;
using OpenTelemetry.Trace;

namespace Nivaes.App.Cross.Sample;

[RequiresUnreferencedCode("Application requires unreferenced code")]
public class SampleApp : CrossApplication, ICrossApplication
{
    private readonly TracerProvider _tracerProvider;

    public SampleApp(IServiceProvider serviceProvider, ILogger<SampleApp> logger, TracerProvider tracerProvider)
        : base(serviceProvider, logger)
    {
        _tracerProvider = tracerProvider;
    }

    public override void Setup()
    {
        base.Setup();
        GeneratedConverterExtensions.RegisterConverters(ServiceProvider);
        GeneratedCombinerExtensions.RegisterCombiners(ServiceProvider);
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

            return new CrossViewModelStar<RootViewModel>();
        }
    }
}
