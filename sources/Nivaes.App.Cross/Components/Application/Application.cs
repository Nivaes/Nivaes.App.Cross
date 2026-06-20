using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Components.ViewModels;

namespace Nivaes.App.Cross.Controls;

public abstract class Application : IApplication
{
    protected ILogger Logger { [DebuggerHidden]get; }

    protected Application(ILogger logger)
    {
        Logger = logger;
    }

    public abstract ICrossViewModelStar Initialize();

    public virtual void Startup()
    {
        
    }

    public virtual void Reset()
    {
    }

    

    //protected void RegisterViewStar<TViewModel>()
    //    where TViewModel : ICrossViewModel
    //{

    //}
}
