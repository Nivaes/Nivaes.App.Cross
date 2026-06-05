using System;
using System.Collections.Generic;
using System.Text;
using Nivaes.App.Cross.Hosting;
using Playground.Core.ViewModels;

namespace Nivaes.App.Cross.Sample.AppKitOS.MacOS;

public static class CrossProgram
{
    public static CrossApp CreateCrossApp()
    {
        var builder = CrossApp.CreateBuilder();

        builder
            .UseSharedCrossApp();

        builder.SetupViews();

        return builder.Build();
    }

    static CrossAppBuilder SetupViews(this CrossAppBuilder builder)
    {
        // ToDo: Cargar esto con roslyn.
        CrossViewsManagerHelper.RegisterViewModel(new[] {
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
