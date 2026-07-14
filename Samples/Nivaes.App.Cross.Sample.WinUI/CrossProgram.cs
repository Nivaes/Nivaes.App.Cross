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
        ViewsContainerManagerHelper.RegisterViewModels(new[] {
                    ViewsContainerManagerHelper.New<RootViewModel, RootView>(),
                    ViewsContainerManagerHelper.New<NewWindowViewModel, NewWindow>(),
                    ViewsContainerManagerHelper.New<ChildViewModel, ChildView>(),
                    ViewsContainerManagerHelper.New<ModalViewModel, DialogView>(),
                    ViewsContainerManagerHelper.New<SecondChildViewModel, SecondChildView>(),
                    ViewsContainerManagerHelper.New<RegionViewModel, RegionView>(),
                    ViewsContainerManagerHelper.New<SplitDetailViewModel, SplitDetailView>(),
                    ViewsContainerManagerHelper.New<SplitMasterViewModel, SplitMasterView>(),
                    ViewsContainerManagerHelper.New<SplitRootViewModel, SplitRootView>(),
                    ViewsContainerManagerHelper.New<ModalViewModel, DialogView>(),
                });

        return builder;
    }
}
