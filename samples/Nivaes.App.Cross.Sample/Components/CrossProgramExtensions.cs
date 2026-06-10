using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Hosting;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Playground.Core.ViewModels;

namespace Nivaes.App.Cross.Sample
{
    public static class CrossProgramExtensions
    {
        public static CrossAppBuilder UseSharedCrossApp(this CrossAppBuilder builder)
        {
            builder
                .UseCrossApp<SampleApp>();

            builder.Services.AddLogging();
            builder.Logging.AddDebug();
            builder.Logging.AddConsole();

            builder.Logging.AddOpenTelemetry(logging =>
            {
                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;

                logging.AddOtlpExporter(o =>
                {
                    o.Protocol =
                        OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;

                    o.Endpoint =
                        new Uri("http://10.0.2.2:4318");
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
                           .AddHttpClientInstrumentation()
                           .AddOtlpExporter(options =>
                           {
                               options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
                               options.Endpoint = new Uri("http://10.0.2.2:4318");
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
                        .AddOtlpExporter(options =>
                        {
                            options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
                            options.Endpoint =
                                new Uri("http://10.0.2.2:4318");
                        });
                });

            return builder;
        }
    }
}
