// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Tvos.Views
{
    using System.Threading.Tasks;
    using MvvmCross.Exceptions;
    using MvvmCross.Views;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.ViewModels;

    public static class MvxViewControllerExtensions
    {
        public static void OnViewCreate(this IMvxTvosView tvOSView)
        {
            //var view = tvOSView as IMvxView<TViewModel>;
            tvOSView.OnViewCreate(tvOSView.LoadViewModel);
        }

        private static async ValueTask<IMvxViewModel> LoadViewModel(this IMvxTvosView tvOSView)
        {
            if (tvOSView.Request == null)
            {
                //MvxLog.Instance.Trace(
                //    "Request is null - assuming this is a TabBar type situation where ViewDidLoad is called during construction... patching the request now - but watch out for problems with virtual calls during construction");
                tvOSView.Request = Mvx.IoCProvider.Resolve<IMvxCurrentRequest>().CurrentRequest;
            }

            var instanceRequest = tvOSView.Request as MvxViewModelInstanceRequest;
            if (instanceRequest != null)
            {
                return instanceRequest.ViewModelInstance;
            }

            var loader = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();
            var viewModel = await loader.LoadViewModel(tvOSView.Request, null /* no saved state on tvOS currently */);
            if (viewModel == null)
                throw new MvxException("ViewModel not loaded for " + tvOSView.Request.ViewModelType);
            return viewModel;
        }
    }
}
