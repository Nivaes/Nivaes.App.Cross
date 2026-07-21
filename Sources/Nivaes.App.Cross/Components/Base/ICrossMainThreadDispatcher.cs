namespace Nivaes.App.Cross
{
    public interface ICrossMainThreadDispatcher
    {
        public void ExceptionMaskedAction(Action action, bool maskExceptions);

        [Obsolete("Use IMainThreadAsyncDispatcher.ExecuteOnMainThreadAsync instead", true)]
        bool RequestMainThreadAction(Action action, bool maskExceptions = true);

        Task ExecuteOnMainThreadAsync(Action action, bool maskExceptions = true);
        Task ExecuteOnMainThreadAsync(Func<Task> action, bool maskExceptions = true);

        bool IsOnMainThread { get; }
    }
}
