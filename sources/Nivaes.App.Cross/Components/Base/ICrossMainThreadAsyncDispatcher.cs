namespace Nivaes.App.Cross
{
    using System;
    using System.Threading.Tasks;

    // Note: The long term goal should be to deprecate IMvxMainThreadDispatcher
    // As such, even though the implementation of this interface also implements
    // IMvxMainThreadDispatcher, this interface should not inherit from IMvxMainThreadDispatcher
    [Obsolete]
    public interface ICrossMainThreadAsyncDispatcher
    {
        Task ExecuteOnMainThreadAsync(Action action, bool maskExceptions = true);
        Task ExecuteOnMainThreadAsync(Func<Task> action, bool maskExceptions = true);
        bool IsOnMainThread { get; }
    }
}
