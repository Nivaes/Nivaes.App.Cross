namespace Nivaes.App.Cross
{
    using System.Threading.Tasks;

    public sealed class NavigationService : INavigationService
    {
        public readonly IViewDispatcher mViewDispatcher;

        public event BeforeNavigateEventHandler BeforeNavigate;

        public event AfterNavigateEventHandler AfterNavigate;

        public event BeforeCloseEventHandler BeforeClose;

        public event AfterCloseEventHandler AfterClose;

        public event BeforeChangePresentationEventHandler BeforeChangePresentation;

        public event AfterChangePresentationEventHandler AfterChangePresentation;


        public NavigationService(IViewDispatcher viewDispatcher)
        {
            mViewDispatcher = viewDispatcher;
        }

        public async Task<bool> Navigate<TViewModel>(ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
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

        public async Task<bool> Navigate<TViewModel, TParameter>(TParameter parameter, ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
            where TViewModel : IViewModel<TParameter>
        {
            return true;
        }

        public async Task<TResult?> Navigate<TViewModel, TResult>(ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
           where TViewModel : IViewModelResult<TResult>
        {
            return default(TResult);
        }

        public async Task<TResult?> Navigate<TViewModel, TParameter, TResult>(TParameter param, ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
            where TViewModel : IViewModel<TParameter, TResult>
        {
            return default(TResult);
        }
    }
}
