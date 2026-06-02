using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Hosting;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Nivaes.App.Cross.Sample
{
    public static class CrossProgramExtensions
    {
        public static CrossAppBuilder UseSharedCrossApp(this CrossAppBuilder builder)
        {
            builder
                .UseCrossApp<SampleApp>();

            /* ToDo: Cargar esto con roslyn */
            builder.Services.AddScoped<BaseViewModel>();
            builder.Services.AddScoped<MainViewModel>();
            builder.Services.AddScoped<NewWindowViewModel>();
            builder.Services.AddScoped<RootViewModel>();

            builder.Services.AddScoped<ChildViewModel>();
            builder.Services.AddScoped<ChildWithResultViewModel>();
            builder.Services.AddScoped<FragmentCloseViewModel>();
            builder.Services.AddScoped<WindowViewModel>();
            builder.Services.AddScoped<WindowChildViewModel>();
            builder.Services.AddScoped<TabsRootViewModel>();
            builder.Services.AddScoped<TabsRootBViewModel>();
            builder.Services.AddScoped<Tab1ViewModel>();
            builder.Services.AddScoped<Tab2ViewModel>();
            builder.Services.AddScoped<Tab3ViewModel>();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddOpenTelemetry()
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
                            options.Endpoint =
                                new Uri("http://10.0.2.2:18889");
                        });
                });

            return builder;
        }
    }
}
