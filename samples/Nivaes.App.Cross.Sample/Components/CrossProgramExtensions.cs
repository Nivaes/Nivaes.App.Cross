using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Hosting;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Playground.Core.ViewModels;

namespace Nivaes.App.Cross.Sample
{
    public static class CrossProgramExtensions
    {
        //const string urlString = "http://10.0.2.2:4318";
        const string urlString = "http://localhost:4318";

        public static CrossAppBuilder UseSharedCrossApp(this CrossAppBuilder builder)
        {
            builder.UseCrossApp<SampleApp>();

            builder.Logging.SetMinimumLevel(LogLevel.Trace);

            builder.Services.AddLogging();
            builder.Logging.AddDebug();
            builder.Logging.AddConsole();

            var listener = new OpenTelemetryEventListener();

            builder.Logging.AddOpenTelemetry(logging =>
            {
                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;

                logging.AddOtlpExporter(o =>
                {
                    o.Protocol = OtlpExportProtocol.HttpProtobuf;
                    o.Endpoint = new Uri(urlString);
                });
            });

            builder.Services.AddOpenTelemetry()
                  .ConfigureResource(r =>
                  {
                      r.AddService(
                          serviceName: "CrossSample",
                          serviceVersion: "0.1");
                  })
                .WithMetrics(metrics =>
                {
                    //metrics.AddAspNetCoreInstrumentation()
                    //    .AddHttpClientInstrumentation()
                    //    .AddRuntimeInstrumentation();
                    metrics.AddRuntimeInstrumentation()
                           //.AddProcessInstrumentation()
                           .AddHttpClientInstrumentation()
                           .AddOtlpExporter(options =>
                           {
                               options.Protocol = OtlpExportProtocol.HttpProtobuf;
                               options.Endpoint = new Uri(urlString);
                           });
                })
                .WithTracing(static tracing =>
                {
                    tracing
                        .SetResourceBuilder(
                            ResourceBuilder.CreateDefault()
                                .AddService(
                                    serviceName: "SampleCrossClient",
                                    serviceVersion: "1.0"))
                        .AddSource("SampleCrossClient")
                        .AddHttpClientInstrumentation()
                        .SetSampler(new AlwaysOnSampler())
                        .AddConsoleExporter()
                        .AddOtlpExporter(options =>
                        {
                            options.Protocol = OtlpExportProtocol.HttpProtobuf;
                            options.Endpoint =
                                new Uri(urlString);
                        });
                });

            AppContext.SetSwitch(
                "OpenTelemetry.Experimental.EnableEventSource",
                true);
            AppContext.SetSwitch(
                "OpenTelemetry.Experimental.EnableEventSource",
                true);

            return builder;
        }
    }
}
