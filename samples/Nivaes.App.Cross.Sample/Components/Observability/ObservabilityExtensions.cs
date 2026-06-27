using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Hosting;
using Nivaes.App.Cross.Observability;
using OpenTelemetry;
using OpenTelemetry.Logs;
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
                    //metrics.AddAspNetCoreInstrumentation()
                    //    .AddHttpClientInstrumentation()
                    //    .AddRuntimeInstrumentation();
                    metrics.AddRuntimeInstrumentation()
                           .AddMeter("Metrica1")
                           .AddMeter(Telemetry.Meter.Name)
                           .AddHttpClientInstrumentation()

                           //.AddProcessInstrumentation()
                           //.AddHttpClientInstrumentation()
                           //.AddOtlpExporter(options =>
                           //{
                           //    options.Protocol = OtlpExportProtocol.HttpProtobuf;
                           //    options.Endpoint = new Uri(urlObservability, "/v1/metrics");
                           //})
                           ;
                })
                .WithTracing(static tracing =>
                {
                    tracing.AddHttpClientInstrumentation()
                           .AddSource("Traza1")
                           .AddSource(Telemetry.ActivitySource.Name)
                        //.SetResourceBuilder(
                        //    ResourceBuilder.CreateDefault()
                        //        .AddService(
                        //            serviceName: "SampleCrossClient",
                        //            serviceVersion: "1.0"))
                        //.AddSource("SampleCrossClient")
                        //.AddHttpClientInstrumentation()
                        //.SetSampler(new AlwaysOnSampler())
                        //.AddConsoleExporter()
                        //.AddOtlpExporter(options =>
                        //{
                        //    options.Protocol = OtlpExportProtocol.HttpProtobuf;
                        //    options.Endpoint = new Uri(urlObservability, "/v1/traces");
                        //})
                        ;
                })
                 .WithLogging(loggersProviderBuilder =>
                 {
                     //loggersProviderBuilder.
                     //loggersProviderBuilder.AddOtlpExporter(options =>
                     //{
                     //    options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
                     //    options.Endpoint = new Uri(urlObservability, "/v1/logs");
                     //});
                 })
            //.UseOtlpExporter(OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf, new Uri("http://localhost:4318"))
            ;

            return openTelemetryBuilder;
        }
    }
}
