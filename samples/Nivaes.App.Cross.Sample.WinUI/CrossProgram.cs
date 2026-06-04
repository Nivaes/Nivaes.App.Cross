using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nivaes.App.Cross.Hosting;
using Nivaes.App.Cross.Sample;
using Nivaes.App.Cross.WinUI;

namespace Nivaes.App.Cross.Sample.WinUI;

public static class CrossProgram
{
    public static CrossApp CreateCrossApp(CrossWinUIApplication app)
    {
        var builder = CrossApp.CreateBuilder();

        builder.UseSharedCrossApp();

        builder.UseWinUIApp(app);

        builder.SetupViews();

        return builder.Build();
    }

    static CrossAppBuilder SetupViews(this CrossAppBuilder builder)
    {
        CrossViewsManagerHelper.RegisterViewModel(new[] {
                    CrossViewsManagerHelper.New<RootViewModel, RootView>(),
                    CrossViewsManagerHelper.New<NewWindowViewModel, NewWindow>(),
                    CrossViewsManagerHelper.New<ChildViewModel, ChildView>(),
                });

        return builder;
    }
}
