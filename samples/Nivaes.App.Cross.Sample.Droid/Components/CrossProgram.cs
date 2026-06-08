using System;
using System.Collections.Generic;
using System.Text;
using Android.Content;
using Microsoft.Extensions.DependencyInjection;
using Nivaes.App.Cross.Droid;
using Nivaes.App.Cross.Hosting;
using Playground.Core.ViewModels;
using Sentry.Protocol;

namespace Nivaes.App.Cross.Sample.Droid;

public static class CrossProgram
{
    public static CrossApp CreateMauiApp(Context context)
    {
        var appBuilder = CrossApp.CreateBuilder();

        appBuilder.UseSharedCrossApp();
        appBuilder.UseDroidApp(context);

        appBuilder.Services.AddMetrics();

        appBuilder.SetupViews();

        return appBuilder.Build();
    }

    static CrossAppBuilder SetupViews(this CrossAppBuilder builder)
    {
        // ToDo: Cargar esto con roslyn.
        CrossViewsManagerHelper.RegisterViewModel(new[] {
                    CrossViewsManagerHelper.New<CollectionViewModel, CollectionView>(),
                    CrossViewsManagerHelper.New<ConvertersViewModel, ConvertersActivity>(),
                    CrossViewsManagerHelper.New<SharedElementRootViewModel, SharedElementRootView>(),
                    CrossViewsManagerHelper.New<SharedElementSecondViewModel, SharedElementSecondView>(),
                    CrossViewsManagerHelper.New<RootViewModel, RootView>(),
                    CrossViewsManagerHelper.New<ChildViewModel, ChildView>(),
                    CrossViewsManagerHelper.New<ChildWithResultViewModel, ChildWithResultFragment>(),
                    CrossViewsManagerHelper.New<DictionaryBindingViewModel, DictionaryBindingView>(),
                    CrossViewsManagerHelper.New<FluentBindingViewModel, FluentBindingView>(),
                    CrossViewsManagerHelper.New<FragmentCloseViewModel, FragmentCloseView>(),
                    CrossViewsManagerHelper.New<ModalNavViewModel, ModalNavView>(),
                    CrossViewsManagerHelper.New<ModalViewModel, ModalView>(),
                    CrossViewsManagerHelper.New<MultiBackStackViewModel, MultiBackStackView>(),
                    CrossViewsManagerHelper.New<NestedChildViewModel, NestedChildView>(),
                    CrossViewsManagerHelper.New<NestedModalViewModel, NestedModalView>(),
                    CrossViewsManagerHelper.New<OverrideAttributeViewModel, OverrideAttributeView>(),
                    CrossViewsManagerHelper.New<SecondChildViewModel, SecondChildView>(),
                    CrossViewsManagerHelper.New<SharedElementRootChildViewModel, SharedElementRootChildView>(),
                    CrossViewsManagerHelper.New<SharedElementSecondChildViewModel, SharedElementSecondChildView>(),
                    CrossViewsManagerHelper.New<SheetViewModel, SheetView>(),
                    CrossViewsManagerHelper.New<SplitDetailNavViewModel, SplitDetailNavView>(),
                    CrossViewsManagerHelper.New<SplitDetailViewModel, SplitDetailView>(),
                    CrossViewsManagerHelper.New<SplitMasterViewModel, SplitMasterView>(),
                    CrossViewsManagerHelper.New<SplitRootViewModel, SplitRootView>(),
                    //CrossViewsManagerHelper.New<CustomBindingViewModel, CustomBindingView>(),
                    CrossViewsManagerHelper.New<ModalNavViewModel, ModalNavView>(),
                    CrossViewsManagerHelper.New<ModalViewModel, ModalView>(),
                    CrossViewsManagerHelper.New<NestedModalViewModel, NestedModalView>(),
                    CrossViewsManagerHelper.New<OverrideAttributeViewModel, OverrideAttributeView>(),
                    CrossViewsManagerHelper.New<Tab1ViewModel, Tab1View>(),
                    CrossViewsManagerHelper.New<Tab2ViewModel, Tab2View>(),
                    CrossViewsManagerHelper.New<Tab3ViewModel, Tab3View>(),
                    CrossViewsManagerHelper.New<TabsRootViewModel, TabsRootView>(),
                    CrossViewsManagerHelper.New<TabsRootBViewModel, TabsRootBView>(),
                });

        return builder;
    }
}
