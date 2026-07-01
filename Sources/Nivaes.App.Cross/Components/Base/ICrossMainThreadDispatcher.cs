namespace Nivaes.App.Cross
{
    using System;

    public interface ICrossMainThreadDispatcher
    {
        [Obsolete("Use IMvxMainThreadAsyncDispatcher.ExecuteOnMainThreadAsync instead")]
        bool RequestMainThreadAction(Action action, bool maskExceptions = true);
        bool IsOnMainThread { get; }
    }
}
