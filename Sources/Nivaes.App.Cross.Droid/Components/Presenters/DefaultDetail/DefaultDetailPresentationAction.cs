using Microsoft.Extensions.Logging;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace Nivaes.App.Cross.Droid;

public sealed class DefaultDetailPresentationAction
    : ViewPagerFragmentPresentationAction<DefaultDetailPresentationAttribute>, IDetailPresentationAction, IFragmentPresentationAction
{
    private IFragmentPresentationAction thisFragment => (IFragmentPresentationAction)this;

    public DefaultDetailPresentationAction(
        PressenterActionContext contex,
            ICrossViewsContainer viewsContainer,
            ICrossNavigationSerializer navigationSerializer,
            ILogger<DefaultDetailPresentationAction> logger)
        : base(contex, viewsContainer,  navigationSerializer, logger)
    {
    }

    protected override ValueTask<bool> ShowAction(Type viewType, DefaultDetailPresentationAttribute attribute, CrossViewModelRequest request)
    {
        var fragmentManager = base.Context.CurrentFragmentManager;
        if (fragmentManager == null)
            return ValueTask.FromResult(true);

        var fragmentHost = ((IDetailPresentationAction)this).FindFragmentHost(attribute.FragmentContentId, fragmentManager);
        if (fragmentHost == null)
            return ValueTask.FromResult(true);

        var fragmentName = attribute.ViewType.FragmentJavaName();

        IMvxFragmentView fragment = (IMvxFragmentView)fragmentManager.FindFragmentByTag(fragmentName);
        fragment = fragment ?? thisFragment.CreateFragment(base.Context.CurrentActivity.SupportFragmentManager, attribute, attribute.ViewType);

        var fragmentView = fragment.ToFragment();
        if (request is CrossViewModelInstanceRequest instanceRequest)
        {
            fragment.ViewModel = instanceRequest.ViewModelInstance;
        }

        var ft = fragmentHost.ChildFragmentManager.BeginTransaction();

        ft.Replace(attribute.FragmentContentId, (Fragment)fragment, fragmentName);
        ft.CommitAllowingStateLoss();

        return ValueTask.FromResult(true);
    }

    protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, DefaultDetailPresentationAttribute attribute)
    {
        return ValueTask.FromResult(true);
    }
}
