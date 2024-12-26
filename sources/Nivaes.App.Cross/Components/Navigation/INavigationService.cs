namespace Nivaes.App.Cross
{
    using System.Threading.Tasks;

    public interface INavigationService
    {
        Task<bool> Navigate<TViewModel>(CancellationToken cancellationToken = default)
            where TViewModel : IViewModel;
    }
}
