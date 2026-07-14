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
        ViewsContainerManagerHelper.RegisterViewModels(new[] {
                    ViewsContainerManagerHelper.New<CollectionViewModel, CollectionView>(),
                    ViewsContainerManagerHelper.New<CrossStartActivityViewModel, SplashScreen>(),
                    ViewsContainerManagerHelper.New<ConvertersViewModel, ConvertersActivity>(),
                    ViewsContainerManagerHelper.New<SharedElementRootViewModel, SharedElementRootView>(),
                    ViewsContainerManagerHelper.New<SharedElementSecondViewModel, SharedElementSecondView>(),
                    ViewsContainerManagerHelper.New<RootViewModel, RootView>(),
                    ViewsContainerManagerHelper.New<ChildViewModel, ChildView>(),
                    ViewsContainerManagerHelper.New<ChildWithResultViewModel, ChildWithResultFragment>(),
                    ViewsContainerManagerHelper.New<DictionaryBindingViewModel, DictionaryBindingView>(),
                    ViewsContainerManagerHelper.New<FluentBindingViewModel, FluentBindingView>(),
                    ViewsContainerManagerHelper.New<FragmentCloseViewModel, FragmentCloseView>(),
                    ViewsContainerManagerHelper.New<ModalNavViewModel, ModalNavView>(),
                    ViewsContainerManagerHelper.New<ModalViewModel, ModalView>(),
                    ViewsContainerManagerHelper.New<MultiBackStackViewModel, MultiBackStackView>(),
                    ViewsContainerManagerHelper.New<NestedChildViewModel, NestedChildView>(),
                    ViewsContainerManagerHelper.New<NestedModalViewModel, NestedModalView>(),
                    ViewsContainerManagerHelper.New<OverrideAttributeViewModel, OverrideAttributeView>(),
                    ViewsContainerManagerHelper.New<SecondChildViewModel, SecondChildView>(),
                    ViewsContainerManagerHelper.New<SharedElementRootChildViewModel, SharedElementRootChildView>(),
                    ViewsContainerManagerHelper.New<SharedElementSecondChildViewModel, SharedElementSecondChildView>(),
                    ViewsContainerManagerHelper.New<SheetViewModel, SheetView>(),
                    ViewsContainerManagerHelper.New<SplitDetailNavViewModel, SplitDetailNavView>(),
                    ViewsContainerManagerHelper.New<SplitDetailViewModel, SplitDetailView>(),
                    ViewsContainerManagerHelper.New<SplitMasterViewModel, SplitMasterView>(),
                    ViewsContainerManagerHelper.New<SplitRootViewModel, SplitRootView>(),
                    ViewsContainerManagerHelper.New<CustomBindingViewModel, CustomBindingView>(),
                    ViewsContainerManagerHelper.New<ModalNavViewModel, ModalNavView>(),
                    ViewsContainerManagerHelper.New<ModalViewModel, ModalView>(),
                    ViewsContainerManagerHelper.New<NestedModalViewModel, NestedModalView>(),
                    ViewsContainerManagerHelper.New<OverrideAttributeViewModel, OverrideAttributeView>(),
                    ViewsContainerManagerHelper.New<Tab1ViewModel, Tab1View>(),
                    ViewsContainerManagerHelper.New<Tab2ViewModel, Tab2View>(),
                    ViewsContainerManagerHelper.New<Tab3ViewModel, Tab3View>(),
                    ViewsContainerManagerHelper.New<TabsRootViewModel, TabsRootView>(),
                    ViewsContainerManagerHelper.New<TabsRootBViewModel, TabsRootBView>(),

                    ViewsContainerManagerHelper.New<MultiBackStackViewModel, MultiBackStackView>(),
                    ViewsContainerManagerHelper.New<MultiBackStackTab1ViewModel, MultiBackStackTab1View>(),
                    ViewsContainerManagerHelper.New<MultiBackStackTab2ViewModel, MultiBackStackTab2View>(),
                    ViewsContainerManagerHelper.New<MultiBackStackInnerViewModel, MultiBackStackInnerView>(),
                });

        return builder;
    }
}
