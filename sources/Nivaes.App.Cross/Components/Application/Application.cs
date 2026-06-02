using System;
using System.Collections.Generic;
using System.Text;
using Nivaes.App.Cross.Components.ViewModels;

namespace Nivaes.App.Cross.Controls;

public abstract class Application : IApplication
{
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
