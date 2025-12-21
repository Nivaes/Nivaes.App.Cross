namespace MvvmCross.Platforms.Tvos.Views
{
    using MvvmCross.Exceptions;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Tvos;

    public static class MvxViewControllerExtensions
    {
        public static void OnViewCreate(this IMvxTvosView tvOSView)
        {
            //var view = tvOSView as IMvxView<TViewModel>;
            tvOSView.OnViewCreate(tvOSView.LoadViewModel);
        }

        private static ICrossViewModel LoadViewModel(this IMvxTvosView tvOSView)
        {
            if (tvOSView.Request == null)
            {
                tvOSView.Request = Mvx.IoCProvider.Resolve<IMvxCurrentRequest>().CurrentRequest;
            }

            var instanceRequest = tvOSView.Request as CrossViewModelInstanceRequest;
            if (instanceRequest != null)
            {
                return instanceRequest.ViewModelInstance;
            }

            var loader = Mvx.IoCProvider.Resolve<ICrossViewModelLoader>();
            var viewModel = loader.LoadViewModel(tvOSView.Request, null /* no saved state on tvOS currently */);
            if (viewModel == null)
                throw new CrossException("ViewModel not loaded for " + tvOSView.Request.ViewModelType);
            return viewModel;
        }
    }
}
