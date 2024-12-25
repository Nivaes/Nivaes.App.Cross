namespace Nivaes.App.Cross
{
    using System.Threading.Tasks;

    public interface INavigationService
    {
        Task Navigate<TViewModel>(CancellationToken cancellationToken = default)
            where TViewModel : IViewModel;
    }
}
