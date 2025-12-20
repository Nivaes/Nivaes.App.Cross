namespace Nivaes.App.Cross
{
    using System;

    [Obsolete()]
    public static class CrossObjectExtensions
    {
        public static void DisposeIfDisposable(this object thing)
        {
            if (thing is IDisposable disposable)
                disposable.Dispose();
        }
    }
}
