using Microsoft.Extensions.Logging;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace Nivaes.App.Cross.Droid;

public sealed class MasterPresentationAction
    : ViewPagerFragmentPresentationAction<MasterPresentationAttribute>, IFragmentPresentationAction
{
    private IFragmentPresentationAction thisAction => (IFragmentPresentationAction)this;

    public MasterPresentationAction(
            IPressenterActionContext contex,
            ILogger<MasterPresentationAction> logger)
        : base(contex, logger)
    {
    }

    protected override ValueTask<bool> ShowAction(IViewModelRequest request, MasterPresentationAttribute attribute)
    {
        if (attribute.FragmentHostViewType != null)
        {
            thisAction.ShowNestedFragment(request.ViewType, attribute, request);

            return ValueTask.FromResult(true); ;
        }

        if (attribute.ActivityHostViewModelType == null)
            attribute.ActivityHostViewModelType = GetCurrentActivityViewModelType();

        var currentHostViewModelType = GetCurrentActivityViewModelType();
        if (attribute.ActivityHostViewModelType != currentHostViewModelType)
        {
            Logger.LogTrace("Activity host with ViewModelType {0} is not CurrentTopActivity. Showing Activity before showing Fragment for {1}",
                attribute.ActivityHostViewModelType, request.ViewModelType);
            base.PendingRequest = request;
            base.ShowHostActivity(attribute);
        }
        else
        {
            if (base.Context.CurrentActivity.FindViewById(attribute.FragmentContentId) == null)
            {
                if (attribute.FragmentHostMasterDetailViewModelType == null)
                    throw new NullReferenceException("FrameLayout to show Fragment not found");

                base.Context.PendingDetailFragmentRequests = request;
                ShowMasterHostFragment(attribute, request);
            }
            else
            {
                if (attribute.FragmentHostMasterDetailViewType != null)
                {
                    var fragmentHost = thisAction.GetFragmentByViewType(attribute.FragmentHostMasterDetailViewType);
                    if (fragmentHost == null)
                        throw new NullReferenceException($"Master frameLayout to show Fragment {request.ViewType.Name} not found");

                    thisAction.PerformShowFragmentTransaction(request, fragmentHost.ChildFragmentManager, attribute);
                }
                else
                {
                    thisAction.PerformShowFragmentTransaction(request, base.Context.CurrentFragmentManager, attribute);
                }
            }
        }

        return ValueTask.FromResult(true);
    }

    private void ShowMasterHostFragment(MasterPresentationAttribute attribute, IViewModelRequest request)
    {
        var viewType = Singleton<ViewModelViewsKeyContainerManager>.Instance
                        .GetValue(attribute.FragmentHostMasterDetailViewModelType);

        if (!viewType.IsSubclassOf(typeof(Fragment)))
            throw new AppException("The host fragment doesnt inherit Fragment");

        var hostViewModelRequest = ViewModelRequest.GetDefaultRequest(attribute.FragmentHostMasterDetailViewModelType);
        base.Show(hostViewModelRequest);
    }
}
