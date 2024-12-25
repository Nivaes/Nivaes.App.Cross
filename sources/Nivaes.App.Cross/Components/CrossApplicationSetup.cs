using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nivaes.App.Cross
{
    public abstract class CrossApplicationSetup : ICrossApplicationSetup, IDisposable
    {
        public void Dispose()
        {
            if (Debugger.IsAttached)
                Debugger.Break();
            else
                Debugger.Launch();
        }
    }
}
