using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Components.ViewModels;

namespace Nivaes.App.Cross;

public abstract class CrossApplication : ICrossApplication
{
    protected IServiceProvider ServiceProvider { [DebuggerHidden] get; }
    protected readonly ILogger Logger;

    protected CrossApplication(IServiceProvider serviceProvider, ILogger logger)
    {
        ServiceProvider = serviceProvider;
        Logger = logger;
    }

    public virtual void Setup()
    {
        GeneratedConverterExtensions.RegisterConverters(ServiceProvider);
        ServiceProvider.SetupCombiners();
    }

    public abstract ICrossViewModelStar Initialize();

    public virtual void Startup()
    {

    }

    public virtual void Reset()
    {
    }
}
