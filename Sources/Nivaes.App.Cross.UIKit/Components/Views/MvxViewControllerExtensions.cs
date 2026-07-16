using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;

namespace Nivaes.App.Cross.UIKitLib;

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
            CrossLoggerHost.GetLogger(nameof(MvxViewControllerExtensions)).LogTrace(
                "LoadViewModelRequest is null - assuming this is a TabBar type situation where ViewDidLoad is called during construction... patching the request now - but watch out for problems with virtual calls during construction");

            var currentRequest = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossCurrentRequest>();
            //if (Mvx.IoCProvider?.TryResolve(out ICrossCurrentRequest? currentRequest) == true &&
            if (currentRequest?.CurrentRequest != null)
            {
                iosView.Request = currentRequest.CurrentRequest;
            }
        }

        if (iosView.Request is CrossViewModelInstanceRequest instanceRequest &&
            instanceRequest.ViewModelInstance != null)
        {
            CrossLoggerHost.GetLogger(nameof(MvxViewControllerExtensions)).LogTrace(
                $"LoadViewModel ({instanceRequest.ViewModelInstance.GetType().Name}) instance already set - returning it directly without loading from locator");
            return instanceRequest.ViewModelInstance;
        }

        var viewModelLoader = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossViewModelLoader>();
        if (iosView.Request != null &&
            viewModelLoader != null)
        {
            var viewModel = viewModelLoader.LoadViewModel(iosView.Request, null /* no saved state on iOS currently */);
            if (viewModel == null)
                throw new AppException($"ViewModel not loaded for {iosView.Request.ViewModelType}");

            CrossLoggerHost.GetLogger(nameof(MvxViewControllerExtensions)).LogTrace(
                $"LoadViewModel loaded ({viewModel.GetType().Name})");
            return viewModel;
        }

        throw new AppException("ViewModel not loaded for null Request on {0}", iosView.GetType().Name);
    }
}
