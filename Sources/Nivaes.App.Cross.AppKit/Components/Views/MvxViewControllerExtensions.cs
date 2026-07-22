using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;

namespace Nivaes.App.Cross.AppKitLib;

public static class MvxViewControllerExtensions
{
    public static void OnViewCreate(this IMvxMacView macView)
    {
        //var view = touchView as IMvxView<TViewModel>;
        macView.OnViewCreate(() => { return macView.LoadViewModel(); });
    }

    private static ICrossViewModel LoadViewModel(this IMvxMacView macView)
    {
        if (macView.Request == null)
        {
            CrossLoggerHost.GetLogger(nameof(MvxViewControllerExtensions)).LogTrace(
                "Request is null - assuming this is a TabBar type situation where ViewDidLoad is called during construction... patching the request now - but watch out for problems with virtual calls during construction");

            macView.Request = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<IMvxCurrentRequest>().CurrentRequest;
        }

        var instanceRequest = macView.Request as ViewModelRequest;
        if (instanceRequest != null)
        {
            return instanceRequest.ViewModel;
        }

        //var loader = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossViewModelLoader>();
        //var viewModel = loader.LoadViewModel(macView.Request, null /* no saved state on iOS currently */);
        //if (viewModel == null)
        //    throw new AppException("ViewModel not loaded for " + macView.Request.ViewModelType);
        //return viewModel;
        return macView.Request.ViewModel;
    }

    [Obsolete("User PressenterAction", true)]
    public static IMvxMacView CreateViewControllerFor<TTargetViewModel>(
        this IMvxMacView view,
        IDictionary<string, string>? parameterValues = null)
        where TTargetViewModel : class, ICrossViewModel
    {
        var parameterBundle = new CrossBundle(parameterValues);
        var request = new ViewModelRequest<TTargetViewModel>(parameterBundle, null);
        return view.CreateViewControllerFor(request);
    }


    [Obsolete("User PressenterAction",true)]
    public static IMvxMacView CreateViewControllerFor(
        this IMvxCanCreateMacView view,
        IViewModelRequest request)
    {
        return IPlatformApplication.Current!.ServiceProvider.GetRequiredService<IMvxMacViewCreator>().CreateView(request);
    }

    [Obsolete("User PressenterAction", true)]
    public static IMvxMacView CreateViewControllerFor(
        this IMvxCanCreateMacView view,Type viewType,
        ViewModelRequest request)
    {
        return IPlatformApplication.Current!.ServiceProvider.GetRequiredService<IMvxMacViewCreator>().CreateViewOfType(viewType, request);
    }

    [Obsolete("User PressenterAction", true)]
    public static IMvxMacView CreateViewControllerFor(
        this IMvxCanCreateMacView view,
        ICrossViewModel viewModel)
    {
        return IPlatformApplication.Current!.ServiceProvider.GetRequiredService<IMvxMacViewCreator>().CreateView(viewModel);
    }
}
