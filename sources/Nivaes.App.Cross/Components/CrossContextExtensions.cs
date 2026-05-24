using Microsoft.Extensions.DependencyInjection;
using Nivaes.App.Cross.Hosting;

namespace Nivaes.App.Cross;

public static class CrossContextExtensions
{
    public static void InitializeAppServices(this CrossApp mauiApp)
    {
        var initServices = mauiApp.Services.GetServices<ICrossInitializeService>();
        if (initServices is null)
            return;

        foreach (var instance in initServices)
            instance.Initialize(mauiApp.Services);
    }

    //public static void InitializeScopedServices(this ICrossContext scopedContext)
    //{
    //    var scopedServices = scopedContext.Services.GetServices<ICrossInitializeScopedService>();
    //    if (scopedServices is null)
    //        return;

    //    foreach (var service in scopedServices)
    //        service.Initialize(scopedContext.Services);
    //}
}
