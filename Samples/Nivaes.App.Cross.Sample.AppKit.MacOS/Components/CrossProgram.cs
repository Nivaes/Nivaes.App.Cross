using Microsoft.Extensions.DependencyInjection;
using Nivaes.App.Cross.AppKitOS;
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
        CrossViewsManagerHelper.RegisterViewModels(new[] {
                    CrossViewsManagerHelper.New<RootViewModel, RootView>(),
                    CrossViewsManagerHelper.New<ModalViewModel, ModalView>(),
                    CrossViewsManagerHelper.New<SheetViewModel, SheetView>(),
                    CrossViewsManagerHelper.New<ChildViewModel, ChildView>(),
                    CrossViewsManagerHelper.New<WindowViewModel,WindowView>(),
                    //CrossViewsManagerHelper.New<ToolbarWindow, ToolbarWindow>(),
                    CrossViewsManagerHelper.New<Tab1ViewModel, Tab1View>(),
                    CrossViewsManagerHelper.New<Tab2ViewModel, Tab2View>(),
                    CrossViewsManagerHelper.New<Tab3ViewModel, Tab3View>(),
                    CrossViewsManagerHelper.New<TabsRootViewModel, TabsRootView>(),
                });

        return builder;
    }
}
