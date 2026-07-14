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

        appBuilder.SetupViews();

        return appBuilder.Build();
    }

    static CrossAppBuilder SetupViews(this CrossAppBuilder builder)
    {
        // ToDo: Cargar esto con roslyn.
        ViewsStoreManager.RegisterViewModels(new[] {
                    ViewsStoreManager.New<RootViewModel, RootView>(),
                    ViewsStoreManager.New<NewWindowViewModel, NewWindow>(),
                    ViewsStoreManager.New<ChildViewModel, ChildView>(),
                    ViewsStoreManager.New<ModalViewModel, DialogView>(),
                    ViewsStoreManager.New<SecondChildViewModel, SecondChildView>(),
                    ViewsStoreManager.New<RegionViewModel, RegionView>(),
                    ViewsStoreManager.New<SplitDetailViewModel, SplitDetailView>(),
                    ViewsStoreManager.New<SplitMasterViewModel, SplitMasterView>(),
                    ViewsStoreManager.New<SplitRootViewModel, SplitRootView>(),
                    ViewsStoreManager.New<ModalViewModel, DialogView>(),
                });

        return builder;
    }
}
