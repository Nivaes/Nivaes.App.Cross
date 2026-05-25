using System;
using System.Collections.Generic;
using System.Text;
using Nivaes.App.Cross.Hosting;

namespace Nivaes.App.Cross
{
    internal interface ICrossContext
    {
        IServiceProvider Services { get; }

        //ICrossHandlersFactory Handlers { get; }
    }
}
