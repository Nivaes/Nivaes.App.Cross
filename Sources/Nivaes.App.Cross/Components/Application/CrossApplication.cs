using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross;

public abstract class CrossApplication : ICrossApplication
{
    protected readonly IServiceProvider ServiceProvider;
    protected readonly ILogger Logger;

    protected CrossApplication(IServiceProvider serviceProvider,
                               ILogger logger)
    {
        ServiceProvider = serviceProvider;
        Logger = logger;
    }

    async void ICrossApplication.Setup()
    {
        await RegisterDatabase();
        Parallel.Invoke(
            RegisterConverters,
            RegisterCombiners
        );
    }

    protected virtual void RegisterConverters() {
        GeneratedConverterExtensions.RegisterConverters(ServiceProvider);
    }

    protected virtual void RegisterCombiners() {
        GeneratedCombinerExtensions.RegisterCombiners(ServiceProvider);
    }

    protected virtual ValueTask RegisterDatabase()
    {
        return ValueTask.CompletedTask;
    }

    public abstract void Initialize();

    public virtual void Startup()
    {

    }

    public virtual void Reset()
    {
    }
}
