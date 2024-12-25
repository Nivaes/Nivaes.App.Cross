namespace Nivaes.App.Cross
{
    using System.Threading.Tasks;

    public interface ICrossApplicationStart
    {
        Task NavigateToFirstViewModel(object? hint = null);
    }
}
