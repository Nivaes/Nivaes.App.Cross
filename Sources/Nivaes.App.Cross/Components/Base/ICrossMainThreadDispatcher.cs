namespace Nivaes.App.Cross
{
    public interface ICrossMainThreadDispatcher
    {
        [Obsolete("Use IMvxMainThreadAsyncDispatcher.ExecuteOnMainThreadAsync instead", true)]
        bool RequestMainThreadAction(Action action, bool maskExceptions = true);
        bool IsOnMainThread { get; }
    }
}
