using Microsoft.Extensions.DependencyInjection;
using Nivaes.App.Cross.Hosting;
using Nivaes.App.Cross.UIKitLib;
using OpenTelemetry;

namespace Nivaes.App.Cross.Sample.UIKitLib
{
    public static class CrossProgram
    {
        public static CrossApp CreateCrossApp(UIWindow windows)
        {
            var appBuilder = CrossApp.CreateBuilder();

            appBuilder.UseSharedCrossApp();

            appBuilder.AddObservability().
                UseOtlpExporter(OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf, new Uri("http://192.168.86.205:4318"));

            appBuilder.UseUIKitApp(windows);

            appBuilder.Services.AddMetrics();

            return appBuilder.Build();
        }
    }
}
