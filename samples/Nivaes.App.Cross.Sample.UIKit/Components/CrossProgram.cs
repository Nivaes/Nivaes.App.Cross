using Microsoft.Extensions.DependencyInjection;
using Nivaes.App.Cross.Hosting;
using Nivaes.App.Cross.UIKitOS;
using OpenTelemetry;
using Playground.Core.ViewModels;
using Playground.iOS.Views;

namespace Nivaes.App.Cross.Sample.UIKitOS
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
            CrossViewsManagerHelper.RegisterViewModels(new[] {
                    CrossViewsManagerHelper.New<RootViewModel, RootView>(),
                    CrossViewsManagerHelper.New<ChildViewModel, ChildView>(),
                    CrossViewsManagerHelper.New<ChildWithResultViewModel,ChildWithResultViewController>(),
                    CrossViewsManagerHelper.New<SecondChildViewModel, SecondChildView>(),
                    CrossViewsManagerHelper.New<SplitDetailNavViewModel, SplitDetailNavView>(),
                    CrossViewsManagerHelper.New<SplitDetailViewModel, SplitDetailView>(),
                    CrossViewsManagerHelper.New<SplitMasterViewModel, SplitMasterView>(),
                    CrossViewsManagerHelper.New<SplitRootViewModel, SplitRootView>(),
                    CrossViewsManagerHelper.New<CustomBindingViewModel, CustomBindingView>(),
                    CrossViewsManagerHelper.New<ModalNavViewModel, ModalNavView>(),
                    CrossViewsManagerHelper.New<ModalViewModel, ModalView>(),
                    CrossViewsManagerHelper.New<NestedModalViewModel, NestedModalView>(),
                    CrossViewsManagerHelper.New<OverrideAttributeViewModel, OverrideAttributeView>(),
                    CrossViewsManagerHelper.New<Page1ViewModel, Page1View>(),
                    CrossViewsManagerHelper.New<Page2ViewModel, Page2View>(),
                    CrossViewsManagerHelper.New<Page3ViewModel, Page3View>(),
                    CrossViewsManagerHelper.New<Tab1ViewModel, Tab1View>(),
                    CrossViewsManagerHelper.New<Tab2ViewModel, Tab2View>(),
                    CrossViewsManagerHelper.New<Tab3ViewModel, Tab3View>(),
                    CrossViewsManagerHelper.New<TabsRootViewModel, TabsRootView>(),
                });

            return builder;
        }
    }
}
