namespace Nivaes.App.Cross
{
    using System.Threading.Tasks;

    [Obsolete]
    public interface ICrossAppStart
    {
        void Start(object? hint = null);

        Task StartAsync(object? hint = null);

        bool IsStarted { get; }

        void ResetStart();
    }
}
