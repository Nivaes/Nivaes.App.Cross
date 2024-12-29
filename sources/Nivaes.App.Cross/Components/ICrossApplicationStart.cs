namespace Nivaes.App.Cross
{
    using System.Threading.Tasks;

    public interface ICrossApplicationStart
    {
        bool IsStarted { get; }

        Task NavigateToFirstViewModel(object? hint = null);
    }
}
