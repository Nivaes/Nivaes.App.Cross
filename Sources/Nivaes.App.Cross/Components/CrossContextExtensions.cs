using Microsoft.Extensions.DependencyInjection;
using Nivaes.App.Cross.Hosting;
using Nivaes.App.Cross.Observability;

namespace Nivaes.App.Cross;

public static class CrossContextExtensions
{
    public static void InitializeAppServices(this CrossApp crossApp)
    {
        crossApp.Services.SetupCrash();

        var initServices = crossApp.Services.GetServices<ICrossInitializeService>();
        if (initServices is null)
            return;

        foreach (var instance in initServices)
            instance.Initialize(crossApp.Services);
    }

    public static ICrossContext MakeApplicationScope<TNativeApplication>(this ICrossContext mauiContext,
            TNativeApplication platformApplication)
        where TNativeApplication : class
    {
        var scopedContext = new CrossContext(mauiContext.Services);

        scopedContext.AddSpecific(platformApplication);

        return scopedContext;
    }
}
