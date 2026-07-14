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

        appBuilder.SetupViews();

        return appBuilder.Build();
    }

    static CrossAppBuilder SetupViews(this CrossAppBuilder builder)
    {
        // ToDo: Cargar esto con roslyn.
        ViewsContainerManagerHelper.RegisterViewModels(new[] {
                    ViewsContainerManagerHelper.New<RootViewModel, RootView>(),
                    ViewsContainerManagerHelper.New<ModalViewModel, ModalView>(),
                    ViewsContainerManagerHelper.New<SheetViewModel, SheetView>(),
                    ViewsContainerManagerHelper.New<ChildViewModel, ChildView>(),
                    ViewsContainerManagerHelper.New<WindowViewModel,WindowView>(),
                    //CrossViewsManagerHelper.New<ToolbarWindow, ToolbarWindow>(),
                    ViewsContainerManagerHelper.New<Tab1ViewModel, Tab1View>(),
                    ViewsContainerManagerHelper.New<Tab2ViewModel, Tab2View>(),
                    ViewsContainerManagerHelper.New<Tab3ViewModel, Tab3View>(),
                    ViewsContainerManagerHelper.New<TabsRootViewModel, TabsRootView>(),
                });

        return builder;
    }
}
