using Microsoft.Extensions.DependencyInjection;
using Nivaes.App.Cross.AppKitLib;
using Nivaes.App.Cross.Hosting;
using OpenTelemetry;

namespace Nivaes.App.Cross.Sample.AppKitOS.MacOS;

public static class CrossProgram
{
    public static CrossApp CreateCrossApp(INSApplicationDelegate applicationDelegation)
    {
        var appBuilder = CrossApp.CreateBuilder();

        appBuilder.UseSharedCrossApp();

        appBuilder.AddObservability().
              UseOtlpExporter(OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf, new Uri("http://192.168.86.205:4318"));

        appBuilder.UseAppKitApp(applicationDelegation);

        appBuilder.Services.AddMetrics();

        return appBuilder.Build();
    }
}
