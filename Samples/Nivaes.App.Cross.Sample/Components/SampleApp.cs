using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Components.ViewModels;
using OpenTelemetry.Trace;

namespace Nivaes.App.Cross.Sample;

public class SampleApp : CrossApplication
{
    private readonly TracerProvider _tracerProvider;

    public SampleApp(IServiceProvider serviceProvider, ILogger<SampleApp> logger, TracerProvider tracerProvider)
        : base(serviceProvider, logger)
    {
        _tracerProvider = tracerProvider;
    }

    protected override void RegisterConverters()
    {
        base.RegisterConverters();
        ServiceProvider.RegisterConverters();
    }

    protected override void RegisterCombiners()
    {
        base.RegisterCombiners();
        //ServiceProvider.RegisterCombiners();
    }

    ///// <summary>
    ///// Breaking change in v6: This method is called on a background thread. Use
    ///// Startup for any UI bound actions
    ///// </summary>
    public override ICrossViewModelStar Initialize()
    {
        using (Logger.BeginScope("Initialice app"))
        {
            var source = new ActivitySource("SampleCross");

            using (var activity = source.StartActivity("SampleCross"))
            {
                activity?.SetTag("test", "true");
            }

            return new CrossViewModelStar<RootViewModel>();
        }
    }
}
