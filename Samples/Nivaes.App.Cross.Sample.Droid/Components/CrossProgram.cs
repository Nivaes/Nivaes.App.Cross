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

        appBuilder.SetupViews();

        return appBuilder.Build();
    }

    static CrossAppBuilder SetupViews(this CrossAppBuilder builder)
    {
        // ToDo: Cargar esto con roslyn.
        ViewsStoreManager.RegisterViewModels(new[] {
                    ViewsStoreManager.New<CollectionViewModel, CollectionView>(),
                    ViewsStoreManager.New<CrossStartActivityViewModel, SplashScreen>(),
                    ViewsStoreManager.New<ConvertersViewModel, ConvertersActivity>(),
                    ViewsStoreManager.New<SharedElementRootViewModel, SharedElementRootView>(),
                    ViewsStoreManager.New<SharedElementSecondViewModel, SharedElementSecondView>(),
                    ViewsStoreManager.New<RootViewModel, RootView>(),
                    ViewsStoreManager.New<ChildViewModel, ChildView>(),
                    ViewsStoreManager.New<ChildWithResultViewModel, ChildWithResultFragment>(),
                    ViewsStoreManager.New<DictionaryBindingViewModel, DictionaryBindingView>(),
                    ViewsStoreManager.New<FluentBindingViewModel, FluentBindingView>(),
                    ViewsStoreManager.New<FragmentCloseViewModel, FragmentCloseView>(),
                    ViewsStoreManager.New<ModalNavViewModel, ModalNavView>(),
                    ViewsStoreManager.New<ModalViewModel, ModalView>(),
                    ViewsStoreManager.New<MultiBackStackViewModel, MultiBackStackView>(),
                    ViewsStoreManager.New<NestedChildViewModel, NestedChildView>(),
                    ViewsStoreManager.New<NestedModalViewModel, NestedModalView>(),
                    ViewsStoreManager.New<OverrideAttributeViewModel, OverrideAttributeView>(),
                    ViewsStoreManager.New<SecondChildViewModel, SecondChildView>(),
                    ViewsStoreManager.New<SharedElementRootChildViewModel, SharedElementRootChildView>(),
                    ViewsStoreManager.New<SharedElementSecondChildViewModel, SharedElementSecondChildView>(),
                    ViewsStoreManager.New<SheetViewModel, SheetView>(),
                    ViewsStoreManager.New<SplitDetailNavViewModel, SplitDetailNavView>(),
                    ViewsStoreManager.New<SplitDetailViewModel, SplitDetailView>(),
                    ViewsStoreManager.New<SplitMasterViewModel, SplitMasterView>(),
                    ViewsStoreManager.New<SplitRootViewModel, SplitRootView>(),
                    ViewsStoreManager.New<CustomBindingViewModel, CustomBindingView>(),
                    ViewsStoreManager.New<ModalNavViewModel, ModalNavView>(),
                    ViewsStoreManager.New<ModalViewModel, ModalView>(),
                    ViewsStoreManager.New<NestedModalViewModel, NestedModalView>(),
                    ViewsStoreManager.New<OverrideAttributeViewModel, OverrideAttributeView>(),
                    ViewsStoreManager.New<Tab1ViewModel, Tab1View>(),
                    ViewsStoreManager.New<Tab2ViewModel, Tab2View>(),
                    ViewsStoreManager.New<Tab3ViewModel, Tab3View>(),
                    ViewsStoreManager.New<TabsRootViewModel, TabsRootView>(),
                    ViewsStoreManager.New<TabsRootBViewModel, TabsRootBView>(),

                    ViewsStoreManager.New<MultiBackStackViewModel, MultiBackStackView>(),
                    ViewsStoreManager.New<MultiBackStackTab1ViewModel, MultiBackStackTab1View>(),
                    ViewsStoreManager.New<MultiBackStackTab2ViewModel, MultiBackStackTab2View>(),
                    ViewsStoreManager.New<MultiBackStackInnerViewModel, MultiBackStackInnerView>(),
                });

        return builder;
    }
}
