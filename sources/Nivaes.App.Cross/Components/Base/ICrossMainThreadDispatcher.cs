namespace Nivaes.App.Cross
{
    using System;

    [Obsolete()]
    public interface ICrossMainThreadDispatcher
    {
        [Obsolete("Use ICrossMainThreadAsyncDispatcher.ExecuteOnMainThreadAsync instead")]
        bool RequestMainThreadAction(Action action, bool maskExceptions = true);
        bool IsOnMainThread { get; }
    }
}
