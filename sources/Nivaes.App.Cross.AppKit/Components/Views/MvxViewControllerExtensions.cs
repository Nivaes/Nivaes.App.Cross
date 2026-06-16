using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.IoC;

namespace Nivaes.App.Cross.AppKitOS;

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
            var logger = IPlatformApplication.Current!.Services.GetRequiredService<ILogger>();
            logger.Log(LogLevel.Trace,
                "Request is null - assuming this is a TabBar type situation where ViewDidLoad is called during construction... patching the request now - but watch out for problems with virtual calls during construction");

            
            macView.Request = IPlatformApplication.Current!.Services.GetRequiredService<IMvxCurrentRequest>().CurrentRequest;
        }

        var instanceRequest = macView.Request as CrossViewModelInstanceRequest;
        if (instanceRequest != null)
        {
            return instanceRequest.ViewModelInstance;
        }

        var loader = IPlatformApplication.Current!.Services.GetRequiredService<ICrossViewModelLoader>();
        var viewModel = loader.LoadViewModel(macView.Request, null /* no saved state on iOS currently */);
        if (viewModel == null)
            throw new CrossException("ViewModel not loaded for " + macView.Request.ViewModelType);
        return viewModel;
    }

    public static IMvxMacView CreateViewControllerFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTargetViewModel>(this IMvxMacView view,
                                                                        object parameterObject)
        where TTargetViewModel : class, ICrossViewModel
    {
        return
            view.CreateViewControllerFor<TTargetViewModel>(parameterObject == null
                                                               ? null
                                                               : parameterObject.ToSimplePropertyDictionary());
    }

#warning TODO - could this move down to IMvxView level?

    public static IMvxMacView CreateViewControllerFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTargetViewModel>(
        this IMvxMacView view,
        IDictionary<string, string> parameterValues = null)
        where TTargetViewModel : class, ICrossViewModel
    {
        var parameterBundle = new CrossBundle(parameterValues);
        var request = new CrossViewModelRequest<TTargetViewModel>(parameterBundle, null);
        return view.CreateViewControllerFor(request);
    }

    public static IMvxMacView CreateViewControllerFor<TTargetViewModel>(
        this IMvxCanCreateMacView view,
        CrossViewModelRequest request)
        where TTargetViewModel : class, ICrossViewModel
    {
        return IPlatformApplication.Current!.Services.GetRequiredService<IMvxMacViewCreator>().CreateView(request);
    }

    public static IMvxMacView CreateViewControllerFor(
        this IMvxCanCreateMacView view,
        CrossViewModelRequest request)
    {
        return IPlatformApplication.Current!.Services.GetRequiredService<IMvxMacViewCreator>().CreateView(request);
    }

    public static IMvxMacView CreateViewControllerFor(
        this IMvxCanCreateMacView view, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type viewType,
        CrossViewModelRequest request)
    {
        return IPlatformApplication.Current!.Services.GetRequiredService<IMvxMacViewCreator>().CreateViewOfType(viewType, request);
    }

    public static IMvxMacView CreateViewControllerFor(
        this IMvxCanCreateMacView view,
        ICrossViewModel viewModel)
    {
        return IPlatformApplication.Current!.Services.GetRequiredService<IMvxMacViewCreator>().CreateView(viewModel);
    }
}
