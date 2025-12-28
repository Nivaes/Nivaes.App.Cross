using Microsoft.Extensions.Logging;
using Nivaes.IoC;

namespace Nivaes.App.Cross.UIKitOS;
public static class MvxViewControllerExtensions
{
    public static void OnViewCreate(this IMvxIosView iosView)
    {
        iosView.OnViewCreate(iosView.LoadViewModel);
    }

    private static ICrossViewModel LoadViewModel(this IMvxIosView iosView)
    {
        if (iosView.Request == null)
        {
            CrossLogHost.Default?.LogTrace(
                "MvxViewControllerExtensions: LoadViewModelRequest is null - assuming this is a TabBar type situation where ViewDidLoad is called during construction... patching the request now - but watch out for problems with virtual calls during construction");

            if (Mvx.IoCProvider?.TryResolve(out ICrossCurrentRequest? currentRequest) == true &&
                currentRequest?.CurrentRequest != null)
            {
                iosView.Request = currentRequest.CurrentRequest;
            }
        }

        if (iosView.Request is CrossViewModelInstanceRequest instanceRequest &&
            instanceRequest.ViewModelInstance != null)
        {
            CrossLogHost.Default?.LogTrace(
                "MvxViewControllerExtensions: LoadViewModel ({ViewModelType}) instance already set - returning it directly without loading from locator",
                instanceRequest.ViewModelInstance.GetType().Name);
            return instanceRequest.ViewModelInstance;
        }

        if (iosView.Request != null &&
            Mvx.IoCProvider?.TryResolve(out ICrossViewModelLoader? viewModelLoader) == true &&
            viewModelLoader != null)
        {
            var viewModel = viewModelLoader.LoadViewModel(iosView.Request, null /* no saved state on iOS currently */);
            if (viewModel == null)
                throw new CrossException($"ViewModel not loaded for {iosView.Request.ViewModelType}");

            CrossLogHost.Default?.LogTrace(
                "MvxViewControllerExtensions: LoadViewModel loaded ({ViewModelType})",
                viewModel.GetType().Name);
            return viewModel;
        }

        throw new CrossException("ViewModel not loaded for null Request on {0}", iosView.GetType().Name);
    }
}
