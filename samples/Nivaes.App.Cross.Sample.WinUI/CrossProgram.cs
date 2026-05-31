using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Nivaes.App.Cross.Hosting;
using Nivaes.App.Cross.Sample;

namespace Nivaes.App.Cross.Sample.WinUI;

public static class CrossProgram
{
    public static CrossApp CreateMauiApp()
    {
        var builder = CrossApp.CreateBuilder();

        builder
            .UseSharedCrossApp();

        /* ToDo: Cargar esto con roslyn */
        builder.Services.AddScoped<BaseViewModel>();
        builder.Services.AddScoped<MainViewModel>();
        builder.Services.AddScoped<NewWindowViewModel>();
        builder.Services.AddScoped<RootViewModel>();

        builder.Services.AddScoped<ChildViewModel>();
        builder.Services.AddScoped<ChildWithResultViewModel>();
        builder.Services.AddScoped<FragmentCloseViewModel>();
        builder.Services.AddScoped<WindowViewModel>();
        builder.Services.AddScoped<WindowChildViewModel>();
        builder.Services.AddScoped<TabsRootViewModel>();
        builder.Services.AddScoped<TabsRootBViewModel>();
        builder.Services.AddScoped<Tab1ViewModel>();
        builder.Services.AddScoped<Tab2ViewModel>();
        builder.Services.AddScoped<Tab3ViewModel>();

        return builder.Build();
    }
}
