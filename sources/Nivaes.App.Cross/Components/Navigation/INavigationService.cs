namespace Nivaes.App.Cross
{
    using System.Threading.Tasks;

    public interface INavigationService
    {
        Task<bool> Navigate<TViewModel>(IBundle? presentationBundle = null, CancellationToken cancellationToken = default)
            where TViewModel : IViewModel;

        Task<bool> Navigate<TViewModel, TParameter>(TParameter parameter, IBundle? presentationBundle = null, CancellationToken cancellationToken = default)
           where TViewModel : IViewModel<TParameter>;

        Task<TResult> Navigate<TViewModel, TResult>(IBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) 
            where TViewModel : IViewModelResult<TResult>;

        Task<TResult> Navigate<TViewModel, TParameter, TResult>(TParameter param, IBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
            where TViewModel : IViewModel<TParameter, TResult>;

    }
}
