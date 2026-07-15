using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross.UIKitLib;

//[Obsolete("1", true)]
public static class MvxCanCreateIosViewExtensions
{
    public static IMvxIosView? CreateViewControllerFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTargetViewModel>(
            this IMvxCanCreateIosView view,
            object parameterObject)
        where TTargetViewModel : class, ICrossViewModel =>
        view.CreateViewControllerFor<TTargetViewModel>(parameterObject.ToSimplePropertyDictionary());

    public static IMvxIosView? CreateViewControllerFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTargetViewModel>(
        this IMvxIosViewCreator viewCreator,
        IDictionary<string, string>? parameterValues = null)
        where TTargetViewModel : class, ICrossViewModel
    {
        var parameterBundle = new CrossBundle(parameterValues);
        var request = new CrossViewModelRequest<TTargetViewModel>(parameterBundle, null);
        return viewCreator.CreateView(request);
    }

    public static IMvxIosView? CreateViewControllerFor(
        this IMvxCanCreateIosView view,
        CrossViewModelRequest request)
    {
        return IPlatformApplication.Current!.ServiceProvider.GetRequiredService<IMvxIosViewCreator>().CreateView(request);
    }

    public static IMvxIosView? CreateViewControllerFor(
        this IMvxCanCreateIosView view, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type viewType)
    {
        return IPlatformApplication.Current!.ServiceProvider.GetRequiredService<IMvxIosViewCreator>().CreateViewOfType(viewType);
    }

    public static IMvxIosView? CreateViewControllerFor(
        this IMvxCanCreateIosView view,
        ICrossViewModel viewModel)
    {
        return IPlatformApplication.Current!.ServiceProvider.GetRequiredService<IMvxIosViewCreator>()?.CreateView(viewModel);
    }
}