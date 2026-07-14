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
            ViewsContainerManagerHelper.RegisterViewModels(new[] {
                    ViewsContainerManagerHelper.New<RootViewModel, RootView>(),
                    ViewsContainerManagerHelper.New<PagesRootViewModel, PagesRootView>(),
                    ViewsContainerManagerHelper.New<ChildViewModel, ChildView>(),
                    ViewsContainerManagerHelper.New<ChildWithResultViewModel,ChildWithResultViewController>(),
                    ViewsContainerManagerHelper.New<SecondChildViewModel, SecondChildView>(),
                    ViewsContainerManagerHelper.New<SplitDetailNavViewModel, SplitDetailNavView>(),
                    ViewsContainerManagerHelper.New<SplitDetailViewModel, SplitDetailView>(),
                    ViewsContainerManagerHelper.New<SplitMasterViewModel, SplitMasterView>(),
                    ViewsContainerManagerHelper.New<SplitRootViewModel, SplitRootView>(),
                    ViewsContainerManagerHelper.New<CustomBindingViewModel, CustomBindingView>(),
                    ViewsContainerManagerHelper.New<ModalNavViewModel, ModalNavView>(),
                    ViewsContainerManagerHelper.New<ModalViewModel, ModalView>(),
                    ViewsContainerManagerHelper.New<NestedModalViewModel, NestedModalView>(),
                    ViewsContainerManagerHelper.New<OverrideAttributeViewModel, OverrideAttributeView>(),
                    ViewsContainerManagerHelper.New<Page1ViewModel, Page1View>(),
                    ViewsContainerManagerHelper.New<Page2ViewModel, Page2View>(),
                    ViewsContainerManagerHelper.New<Page3ViewModel, Page3View>(),
                    ViewsContainerManagerHelper.New<Tab1ViewModel, Tab1View>(),
                    ViewsContainerManagerHelper.New<Tab2ViewModel, Tab2View>(),
                    ViewsContainerManagerHelper.New<Tab3ViewModel, Tab3View>(),
                    ViewsContainerManagerHelper.New<TabsRootViewModel, TabsRootView>(),
                });

            return builder;
        }
    }
}
