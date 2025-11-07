namespace Nivaes.App.Cross
{
    using System;

    public static class CrossObjectExtensions
    {
        public static void DisposeIfDisposable(this object thing)
        {
            if (thing is IDisposable disposable)
                disposable.Dispose();
        }
    }
}
