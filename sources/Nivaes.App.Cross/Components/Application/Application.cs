using System;
using System.Collections.Generic;
using System.Text;

namespace Nivaes.App.Cross.Controls;

public abstract class Application : IApplication
{
    public abstract void Initialize();

    public virtual void Startup()
    {
    }

    public virtual void Reset()
    {
    }
}
