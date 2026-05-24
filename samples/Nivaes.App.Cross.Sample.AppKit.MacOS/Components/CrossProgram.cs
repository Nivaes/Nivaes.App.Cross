using System;
using System.Collections.Generic;
using System.Text;
using Nivaes.App.Cross.Hosting;

namespace Nivaes.App.Cross.Sample.AppKitOS.MacOS;

public static class CrossProgram
{
    public static CrossApp CreateCrossApp()
    {
        var builder = CrossApp.CreateBuilder();

        builder
            .UseSharedCrossApp();

        return builder.Build();
    }
}
