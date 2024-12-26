namespace Nivaes.App.Cross
{
    using System;
    using System.Diagnostics;

    public abstract class CrossApplicationSetup : ICrossApplicationSetup, IDisposable
    {
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Debugger.IsAttached)
                Debugger.Break();
            else
                Debugger.Launch();
        }
    }
}
