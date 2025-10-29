namespace Nivaes.App.Cross
{
    using Nivaes.IoC;

    public static class CrossViewModelLoader
    {
        public static TViewModel LoadViewModel<TViewModel>()
             where TViewModel : ICrossViewModel
        {
            var container = Singleton<CrossIoCServiceContainer>.Instance;
            var viewModel = container.Resolve<TViewModel>();

            if (viewModel == null)
            {
                throw new CrossException($"No se pudo crear el ViewModel of type {typeof(TViewModel)}.");
            }

            return viewModel;
        }

        public static ICrossViewModel ReloadViewModel<TViewModel, TParameter>(ICrossViewModel<TParameter> viewModel, 
            TParameter param, CrossViewModelRequest<TViewModel> request, ICrossBundle savedState, ICrossNavigateEventArgs? navigationArgs = null)
            where TViewModel : ICrossViewModel
        {
            throw new NotImplementedException();
        }
    }
}
