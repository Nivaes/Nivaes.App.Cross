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

    void ICrossApplication.Setup()
    {
        RegisterConverters();
        RegisterCombiners();
    }

    protected virtual void RegisterConverters() {
        GeneratedConverterExtensions.RegisterConverters(ServiceProvider);
    }

    protected virtual void RegisterCombiners() {
        GeneratedCombinerExtensions.RegisterCombiners(ServiceProvider);
    }

    public abstract ICrossViewModelStar Initialize();

    public virtual void Startup()
    {

    }

    public virtual void Reset()
    {
    }

   
}
