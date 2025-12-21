namespace Nivaes.App.Cross
{
    public interface ICrossAsyncCommand : ICrossCommand
    {
        Task ExecuteAsync(object? parameter = null);
        void Cancel();
    }

    public interface ICrossAsyncCommand<in TParameter> : ICrossCommand<TParameter>
    {
        Task ExecuteAsync(TParameter parameter);
        void Cancel();
    }
}