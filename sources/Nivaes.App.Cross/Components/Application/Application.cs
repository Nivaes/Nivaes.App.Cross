using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Components.ViewModels;

namespace Nivaes.App.Cross.Controls;

public abstract class Application : IApplication
{
    protected IServiceProvider ServiceProvider { [DebuggerHidden] get; }
    protected ILogger Logger { [DebuggerHidden] get; }

    protected Application(IServiceProvider serviceProvider, ILogger logger)
    {
        ServiceProvider = serviceProvider;
        Logger = logger;
    }

    public virtual void Setup()
    {
        ServiceProvider.SetupConverters();
        ServiceProvider.SetupCombertes();
    }

    public abstract ICrossViewModelStar Initialize();

    public virtual void Startup()
    {

    }

    public virtual void Reset()
    {
    }
}
