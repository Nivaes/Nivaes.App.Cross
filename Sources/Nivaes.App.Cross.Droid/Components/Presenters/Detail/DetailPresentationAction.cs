using Microsoft.Extensions.Logging;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace Nivaes.App.Cross.Droid;

public sealed class DetailPresentationAction
    : ViewPagerFragmentPresentationAction<DetailPresentationAttribute>, IDetailPresentationAction, IFragmentPresentationAction
{
    private IDetailPresentationAction thisAction => (IDetailPresentationAction)this;
    private IFragmentPresentationAction thisFragment => (IFragmentPresentationAction)this;

    public DetailPresentationAction(
            IPressenterActionContext contex,
            IMvxAndroidViewModelRequestTranslator viewModelRequestTranslator,
            ILogger<DetailPresentationAction> logger)
        : base(contex, logger)
    {
    }

    protected override ValueTask<bool> ShowAction(IViewModelRequest request, DetailPresentationAttribute attribute)
    {
        var detailView = base.Context.CurrentActivity.FindViewById(attribute.FragmentContentId);

        if (detailView == null || base.PendingRequest != null)
        {
            ShowAlternativeDetailFragment(attribute, request);
        }
        else
        {
            ShowEmbeddendDetailFragment(attribute, request);
        }

        return ValueTask.FromResult(true);
    }

    private void ShowAlternativeDetailFragment(
            DetailPresentationAttribute attribute,
            IViewModelRequest request)
    {
        if (base.PendingRequest == null)
        {
            base.PendingRequest = request;
            ShowAlternativeDetailHostActivity(attribute);

            return;
        }

        if (attribute.AlternativeDetailActivityHostViewModelType == null)
            attribute.AlternativeDetailActivityHostViewModelType = GetCurrentActivityViewModelType();

        var currentHostViewModelType = GetCurrentActivityViewModelType();
        if (attribute.AlternativeDetailActivityHostViewModelType != currentHostViewModelType)
        {
            Logger.LogTrace("Activity host with ViewModelType {0} is not CurrentTopActivity. Showing Activity before showing Fragment for {1}",
                attribute.AlternativeDetailActivityHostViewModelType, request.ViewModelType);
            base.PendingRequest = request;
            base.ShowHostActivity(attribute);
        }
        else
        {
            if (base.Context.CurrentActivity.FindViewById(attribute.FragmentContentId) == null)
                throw new NullReferenceException("FrameLayout to show Fragment not found");

            thisFragment.PerformShowFragmentTransaction(request, base.Context.CurrentFragmentManager!, attribute);
        }
    }

    private void ShowEmbeddendDetailFragment(
           DetailPresentationAttribute attribute,
           IViewModelRequest request)
    {
        ArgumentNullException.ThrowIfNull(attribute);

        var fragmentManager = base.Context.CurrentFragmentManager;

        var fragmentHost = ((IDetailPresentationAction)this).FindFragmentHost(attribute.FragmentContentId, fragmentManager!);
        if (fragmentHost == null)
            return;

        var fragmentName = request.ViewType.FragmentJavaName();

        var fragment = (IMvxFragmentView?)fragmentManager!.FindFragmentByTag(fragmentName);
        fragment = fragment ?? thisFragment.CreateFragment(base.Context.CurrentActivity.SupportFragmentManager, attribute, request.ViewType);

        var fragmentView = fragment.ToFragment();
        fragment.ViewModel = request.ViewModel;

        var ft = fragmentHost.ChildFragmentManager.BeginTransaction();

        ft.Replace(attribute.FragmentContentId, (Fragment)fragment, fragmentName);
        ft.CommitAllowingStateLoss();
    }

    private void ShowAlternativeDetailHostActivity(DetailPresentationAttribute attribute)
    {
        var viewType = Singleton<ViewsContainers>.Instance.ViewModelViews[attribute.AlternativeDetailActivityHostViewModelType!];

        if (!viewType.IsSubclassOf(typeof(Android.App.Activity)))
            throw new AppException("The host activity doesn't inherit Activity");

        var hostViewModelRequest = ViewModelRequest.GetDefaultRequest(attribute.AlternativeDetailActivityHostViewModelType!);
        hostViewModelRequest.PresentationValues = base.PendingRequest!.PresentationValues;
        Show(hostViewModelRequest);
    }
}
