using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Components.ViewModels;
using Serilog.Core;

namespace Nivaes.App.Cross.Controls;

public abstract class Application : IApplication
{
    protected ILogger Logger { get; private set; }

    public Application(ILogger logger)
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
