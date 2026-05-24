using System;
using System.Collections.Generic;
using System.Text;
using Nivaes.App.Cross.Hosting;
using Nivaes.App.Cross.Sample;

namespace Nivaes.App.Cross.Sample.WinUI3;

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
