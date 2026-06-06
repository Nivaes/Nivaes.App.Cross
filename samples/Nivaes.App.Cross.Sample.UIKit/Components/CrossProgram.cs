using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Nivaes.App.Cross.Hosting;
using Nivaes.App.Cross.UIKitOS;
using Playground.Core.ViewModels;
using Playground.iOS.Views;
using Sentry.Protocol;

namespace Nivaes.App.Cross.Sample.UIKitOS
{
    public static class CrossProgram
    {
        public static CrossApp CreateCrossApp()
        {
            var appBuilder = CrossApp.CreateBuilder();

            appBuilder.UseSharedCrossApp();
            appBuilder.UseUIKitApp(/*app*/);

            appBuilder.Services.AddMetrics();

            appBuilder.SetupViews();

            return appBuilder.Build();
        }

        static CrossAppBuilder SetupViews(this CrossAppBuilder builder)
        {
            // ToDo: Cargar esto con roslyn.
            CrossViewsManagerHelper.RegisterViewModel(new[] {
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
