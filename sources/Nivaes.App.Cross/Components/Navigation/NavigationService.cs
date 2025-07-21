namespace Nivaes.App.Cross
{
    using System.Threading.Tasks;

    public class NavigationService : INavigationService
    {
        protected readonly IViewDispatcher mViewDispatcher;

        public NavigationService(IViewDispatcher viewDispatcher)
        {
            mViewDispatcher = viewDispatcher;
        }

        public async Task<bool> Navigate<TViewModel>(IBundle? presentationBundle = null, CancellationToken cancellationToken = default)
            where TViewModel : IViewModel
        {
            var viewModel = ViewModelLoader.LoadViewModel<TViewModel>();
            if (viewModel == null)
            {
                throw new CrossException($"No se pudo crear el ViewModel of type {typeof(TViewModel)}.");
            }

            IViewModelRequest request = new ViewModelRequest<TViewModel>(viewModel);

            var hasNavigated = await mViewDispatcher.ShowViewModel(request).ConfigureAwait(false);

            if (!hasNavigated)
                return false;

            return true;
        }

        public async Task<bool> Navigate<TViewModel, TParameter>(TParameter parameter, IBundle? presentationBundle = null, CancellationToken cancellationToken = default)
            where TViewModel : IViewModel<TParameter>
        {
            return true;
        }

        public async Task<TResult> Navigate<TViewModel, TResult>(IBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
           where TViewModel : IViewModelResult<TResult>
        {
            return default(TResult);
        }

        public async Task<TResult> Navigate<TViewModel, TParameter, TResult>(TParameter param, IBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
            where TViewModel : IViewModel<TParameter, TResult>
        {
            return default(TResult);
        }
    }
}
