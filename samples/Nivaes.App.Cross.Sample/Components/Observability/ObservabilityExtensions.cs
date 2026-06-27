using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Hosting;
using Nivaes.App.Cross.Observability;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Nivaes.App.Cross.Sample
{
    public static class ObservabilityExtensions
    {
        public static OpenTelemetryBuilder AddObservability(this CrossAppBuilder builder)
        {
            builder.Logging.SetMinimumLevel(LogLevel.Trace);

            builder.Services.AddLogging();
            builder.Logging.AddDebug();
            builder.Logging.AddSimpleConsole(options =>
            {
                options.IncludeScopes = true;
            });

            builder.Logging.AddOpenTelemetry(logging =>
            {
                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;
            });

            var openTelemetryBuilder = builder.Services.AddOpenTelemetry()
                .ConfigureResource(r =>
                {
                    r.AddService(
                        serviceName: $"CrossSample: {RuntimeInformation.OSDescription}",
                        serviceVersion: "{0.1}");
                })
                .WithMetrics(metrics =>
                {
                    metrics.AddRuntimeInstrumentation()
                           .AddMeter("Metrica1")
                           .AddMeter(Telemetry.Meter.Name)
                           .AddHttpClientInstrumentation();
                })
                .WithTracing(static tracing =>
                {
                    tracing.AddHttpClientInstrumentation()
                           .AddSource("Traza1")
                           .AddSource(Telemetry.ActivitySource.Name);
                })
                .WithLogging();

            return openTelemetryBuilder;
        }
    }
}
