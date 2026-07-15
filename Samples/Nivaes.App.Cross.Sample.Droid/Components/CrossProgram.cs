using Android.Content;
using Microsoft.Extensions.DependencyInjection;
using Nivaes.App.Cross.Droid;
using Nivaes.App.Cross.Hosting;
using OpenTelemetry;

namespace Nivaes.App.Cross.Sample.Droid;

public static class CrossProgram
{
    public static CrossApp CreateCrossApp(Context context)
    {
        var appBuilder = CrossApp.CreateBuilder();

        appBuilder.UseSharedCrossApp();

        appBuilder.AddObservability().
            UseOtlpExporter(OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf, new Uri("http://10.0.2.2:4318"));

        appBuilder.UseDroidApp(context);

        appBuilder.Services.AddMetrics();

        return appBuilder.Build();
    }
}
