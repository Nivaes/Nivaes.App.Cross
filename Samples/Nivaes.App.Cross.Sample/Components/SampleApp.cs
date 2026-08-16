using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;

namespace Nivaes.App.Cross.Sample;

public class SampleApp 
    : CrossApplication
{
    private readonly CrossNavigationService NavigationService;
    private readonly TracerProvider _tracerProvider;

    public SampleApp(IServiceProvider serviceProvider, CrossNavigationService navigationService,
            ILogger<SampleApp> logger, TracerProvider tracerProvider)
        : base(serviceProvider, logger)
    {
        NavigationService = navigationService;  
        _tracerProvider = tracerProvider;
    }

    protected override void RegisterConverters()
    {
        base.RegisterConverters();
        GeneratedConverterExtensions.RegisterConverters(ServiceProvider);
    }

    protected override void RegisterCombiners()
    {
        base.RegisterCombiners();
        GeneratedCombinerExtensions.RegisterCombiners(ServiceProvider);
    }

    public override void Initialize()
    {
        using (Logger.BeginScope("Initialice app"))
        {
            var source = new ActivitySource("SampleCross");

            using (var activity = source.StartActivity("SampleCross"))
            {
                activity?.SetTag("test", "true");
            }

            NavigationService.Navigate<RootViewModel>();
        }
    }
}
