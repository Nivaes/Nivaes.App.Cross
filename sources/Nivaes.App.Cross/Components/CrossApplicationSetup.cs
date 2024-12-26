namespace Nivaes.App.Cross
{
    using System;
    using System.Diagnostics;
    using Nivaes.IoC;

    public abstract class CrossApplicationSetup : ICrossApplicationSetup, IDisposable
    {
        protected CrossApplicationSetup()
        {
        }

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
