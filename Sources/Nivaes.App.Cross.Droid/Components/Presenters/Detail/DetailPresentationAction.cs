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
            ICrossViewsContainer viewsContainer,
            IMvxAndroidViewModelRequestTranslator viewModelRequestTranslator,
            ICrossNavigationSerializer navigationSerializer,
            ILogger<DetailPresentationAction> logger)
        : base(contex, viewsContainer, navigationSerializer, logger)
    {
    }

    protected override ValueTask<bool> ShowAction(Type viewType, DetailPresentationAttribute attribute, CrossViewModelRequest request)
    {
        var detailView = base.Context.CurrentActivity.FindViewById(attribute.FragmentContentId);

        if (detailView == null || base.PendingRequest != null)
        {
            ShowAlternativeDetailFragment(viewType, attribute, request);
        }
        else
        {
            ShowEmbeddendDetailFragment(viewType, attribute, request);
        }

        return ValueTask.FromResult(true);
    }

    private void ShowAlternativeDetailFragment(Type viewType,
            DetailPresentationAttribute attribute,
            CrossViewModelRequest request)
    {
        ArgumentNullException.ThrowIfNull(attribute);

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
                attribute.AlternativeDetailActivityHostViewModelType, attribute.ViewModelType);
            base.PendingRequest = request;
            base.ShowHostActivity(attribute);
        }
        else
        {
            if (base.Context.CurrentActivity.FindViewById(attribute.FragmentContentId) == null)
                throw new NullReferenceException("FrameLayout to show Fragment not found");

            thisFragment.PerformShowFragmentTransaction(base.Context.CurrentFragmentManager, attribute, request);
        }
    }

    private void ShowEmbeddendDetailFragment(
           Type viewType,
           DetailPresentationAttribute attribute,
           CrossViewModelRequest request)
    {
        ArgumentNullException.ThrowIfNull(attribute);

        var fragmentManager = base.Context.CurrentFragmentManager;

        var fragmentHost = ((IDetailPresentationAction)this).FindFragmentHost(attribute.FragmentContentId, fragmentManager);
        if (fragmentHost == null)
            return;

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
    }

    private void ShowAlternativeDetailHostActivity(DetailPresentationAttribute attribute)
    {
        if (attribute == null) throw new ArgumentNullException(nameof(attribute));

        var viewType = ViewsContainer.GetViewType(attribute.AlternativeDetailActivityHostViewModelType);
        if (!viewType.IsSubclassOf(typeof(Android.App.Activity)))
            throw new AppException("The host activity doesn't inherit Activity");

        var hostViewModelRequest = CrossViewModelRequest.GetDefaultRequest(attribute.AlternativeDetailActivityHostViewModelType);
        hostViewModelRequest.PresentationValues = base.PendingRequest.PresentationValues;
        Show(hostViewModelRequest);
    }
}
