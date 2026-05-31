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

        return builder.Build();
    }
}
