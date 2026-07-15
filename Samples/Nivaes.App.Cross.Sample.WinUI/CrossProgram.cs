using Microsoft.Extensions.DependencyInjection;
using Nivaes.App.Cross.Hosting;
using Nivaes.App.Cross.WinUI;
using OpenTelemetry;

namespace Nivaes.App.Cross.Sample.WinUI;

public static class CrossProgram
{
    public static CrossApp CreateCrossApp(CrossWinUIApplication app)
    {
        var appBuilder = CrossApp.CreateBuilder();

        appBuilder.UseSharedCrossApp();

        appBuilder.AddObservability().
           UseOtlpExporter(OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf, new Uri("http://localhost:4318"));

        appBuilder.UseWinUIApp(app);

        appBuilder.Services.AddMetrics();

        return appBuilder.Build();
    }   
}
