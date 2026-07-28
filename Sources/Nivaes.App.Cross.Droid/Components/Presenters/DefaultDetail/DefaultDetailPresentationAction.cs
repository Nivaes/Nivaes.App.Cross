using Microsoft.Extensions.Logging;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace Nivaes.App.Cross.Droid;

public sealed class DefaultDetailPresentationAction
    : ViewPagerFragmentPresentationAction<DefaultDetailPresentationAttribute>, IDetailPresentationAction, IFragmentPresentationAction
{
    private IFragmentPresentationAction thisFragment => (IFragmentPresentationAction)this;

    public DefaultDetailPresentationAction(
            IPressenterActionContext contex,
            ILogger<DefaultDetailPresentationAction> logger)
        : base(contex, logger)
    {
    }

    protected override ValueTask<bool> ShowAction(IViewModelRequest request, DefaultDetailPresentationAttribute attribute)
    {
        var fragmentManager = base.Context.CurrentFragmentManager;
        if (fragmentManager == null)
            return ValueTask.FromResult(true);

        var fragmentHost = ((IDetailPresentationAction)this).FindFragmentHost(attribute.FragmentContentId, fragmentManager);
        if (fragmentHost == null)
            return ValueTask.FromResult(true);

        var fragmentName = request.ViewType.FragmentJavaName();

        IMvxFragmentView fragment = (IMvxFragmentView)fragmentManager.FindFragmentByTag(fragmentName);
        fragment = fragment ?? thisFragment.CreateFragment(base.Context.CurrentActivity.SupportFragmentManager, attribute, request.ViewType);

        var fragmentView = fragment.ToFragment();
        //if (request is CrossViewModelInstanceRequest instanceRequest)
        //{
        //    fragment.ViewModel = instanceRequest.ViewModelInstance;
        //}
        fragment.ViewModel = request.ViewModel;

        var ft = fragmentHost.ChildFragmentManager.BeginTransaction();

        ft.Replace(attribute.FragmentContentId, (Fragment)fragment, fragmentName);
        ft.CommitAllowingStateLoss();

        return ValueTask.FromResult(true);
    }

    protected override ValueTask<bool> CloseAction(IViewModelRequest request, DefaultDetailPresentationAttribute attribute)
    {
        return ValueTask.FromResult(true);
    }
}
