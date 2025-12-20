namespace Nivaes.App.Cross
{
    using System;
    using System.Threading.Tasks;

    [Obsolete()]
    public interface ICrossMainThreadAsyncDispatcher
    {
        Task ExecuteOnMainThreadAsync(Action action, bool maskExceptions = true);
        Task ExecuteOnMainThreadAsync(Func<Task> action, bool maskExceptions = true);
        bool IsOnMainThread { get; }
    }
}
