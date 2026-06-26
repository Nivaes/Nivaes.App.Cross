using Microsoft.Extensions.DependencyInjection;
using Nivaes.App.Cross.Hosting;
using Nivaes.App.Cross.WinUI;
using OpenTelemetry;

namespace Nivaes.App.Cross.Sample.WinUI;

public static class CrossProgram
{
    public static CrossApp CreateCrossApp(CrossApplication app)
    {
        var appBuilder = CrossApp.CreateBuilder();

        appBuilder.UseSharedCrossApp();

        appBuilder.AddObservability().
           UseOtlpExporter(OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf, new Uri("http://localhost:4318"));

        appBuilder.UseWinUIApp(app);

        appBuilder.Services.AddMetrics();

        appBuilder.SetupViews();

        return appBuilder.Build();
    }

    static CrossAppBuilder SetupViews(this CrossAppBuilder builder)
    {
        // ToDo: Cargar esto con roslyn.
        CrossViewsManagerHelper.RegisterViewModels(new[] {
                    CrossViewsManagerHelper.New<RootViewModel, RootView>(),
                    CrossViewsManagerHelper.New<NewWindowViewModel, NewWindow>(),
                    CrossViewsManagerHelper.New<ChildViewModel, ChildView>(),
                    CrossViewsManagerHelper.New<ModalViewModel, DialogView>(),
                    CrossViewsManagerHelper.New<SecondChildViewModel, SecondChildView>(),
                    CrossViewsManagerHelper.New<RegionViewModel, RegionView>(),
                    CrossViewsManagerHelper.New<SplitDetailViewModel, SplitDetailView>(),
                    CrossViewsManagerHelper.New<SplitMasterViewModel, SplitMasterView>(),
                    CrossViewsManagerHelper.New<SplitRootViewModel, SplitRootView>(),
                    CrossViewsManagerHelper.New<ModalViewModel, DialogView>(),
                });

        return builder;
    }
}
