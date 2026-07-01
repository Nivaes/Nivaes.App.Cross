using System.Diagnostics.CodeAnalysis;
using Nivaes.IoC;

namespace Nivaes.App.Cross.UIKitOS;

[Obsolete("1", true)]
public static class MvxCanCreateIosViewExtensions
{
    public static IMvxIosView? CreateViewControllerFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTargetViewModel>(
            this IMvxCanCreateIosView view,
            object parameterObject)
        where TTargetViewModel : class, ICrossViewModel =>
        view.CreateViewControllerFor<TTargetViewModel>(parameterObject.ToSimplePropertyDictionary());

    // TODO - could this move down to IMvxView level?
    public static IMvxIosView? CreateViewControllerFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTargetViewModel>(
        this IMvxIosViewCreator viewCreator,
        IDictionary<string, string>? parameterValues = null)
        where TTargetViewModel : class, ICrossViewModel
    {
        var parameterBundle = new CrossBundle(parameterValues);
        var request = new CrossViewModelRequest<TTargetViewModel>(parameterBundle, null);
        return viewCreator.CreateView(request);
    }
}