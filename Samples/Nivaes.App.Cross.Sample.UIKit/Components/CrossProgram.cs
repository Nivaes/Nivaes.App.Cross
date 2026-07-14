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

            appBuilder.SetupViews();

            return appBuilder.Build();
        }

        static CrossAppBuilder SetupViews(this CrossAppBuilder builder)
        {
            // ToDo: Cargar esto con roslyn.
            ViewsStoreManager.RegisterViewModels(new[] {
                    ViewsStoreManager.New<RootViewModel, RootView>(),
                    ViewsStoreManager.New<PagesRootViewModel, PagesRootView>(),
                    ViewsStoreManager.New<ChildViewModel, ChildView>(),
                    ViewsStoreManager.New<ChildWithResultViewModel,ChildWithResultViewController>(),
                    ViewsStoreManager.New<SecondChildViewModel, SecondChildView>(),
                    ViewsStoreManager.New<SplitDetailNavViewModel, SplitDetailNavView>(),
                    ViewsStoreManager.New<SplitDetailViewModel, SplitDetailView>(),
                    ViewsStoreManager.New<SplitMasterViewModel, SplitMasterView>(),
                    ViewsStoreManager.New<SplitRootViewModel, SplitRootView>(),
                    ViewsStoreManager.New<CustomBindingViewModel, CustomBindingView>(),
                    ViewsStoreManager.New<ModalNavViewModel, ModalNavView>(),
                    ViewsStoreManager.New<ModalViewModel, ModalView>(),
                    ViewsStoreManager.New<NestedModalViewModel, NestedModalView>(),
                    ViewsStoreManager.New<OverrideAttributeViewModel, OverrideAttributeView>(),
                    ViewsStoreManager.New<Page1ViewModel, Page1View>(),
                    ViewsStoreManager.New<Page2ViewModel, Page2View>(),
                    ViewsStoreManager.New<Page3ViewModel, Page3View>(),
                    ViewsStoreManager.New<Tab1ViewModel, Tab1View>(),
                    ViewsStoreManager.New<Tab2ViewModel, Tab2View>(),
                    ViewsStoreManager.New<Tab3ViewModel, Tab3View>(),
                    ViewsStoreManager.New<TabsRootViewModel, TabsRootView>(),
                });

            return builder;
        }
    }
}
