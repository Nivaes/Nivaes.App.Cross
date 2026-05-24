using System;
using System.Collections.Generic;
using System.Text;
using Nivaes.App.Cross.Hosting;

namespace Nivaes.App.Cross.Sample.Droid;

public static class CrossProgram
{
    public static CrossApp CreateMauiApp()
    {
        var builder = CrossApp.CreateBuilder();

        builder
            .UseSharedCrossApp();

        //builder.Services.AddSingleton<IPersonService, PersonService>();

        return builder.Build();
    }
}
