using Microsoft.Extensions.Logging;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace Nivaes.App.Cross.Droid
{
    public sealed class FullPresentationAction
        : ViewPagerFragmentPresentationAction<FullPresentationAttribute>, IFragmentPresentationAction
    {
        private IFragmentPresentationAction thisFragment => (IFragmentPresentationAction)this;

        public FullPresentationAction(
                IPressenterActionContext context,
                ILogger<FullPresentationAction> logger)
            : base(context, logger)
        {
        }

        // ToDo: Poner ICrossPresentationAttribute como generico.
        protected override ValueTask<bool> ShowAction(IViewModelRequest request, FullPresentationAttribute attribute)
        {
            if (attribute.FragmentHostViewType != null)
            {
                thisFragment.ShowNestedFragment(request.ViewType, attribute, request);

                return ValueTask.FromResult(true);
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
                Fragment fragmentHost;
                if (attribute.FragmentHostFullView != null &&
                    (fragmentHost = thisFragment.GetFragmentByViewType(attribute.FragmentHostFullView)) != null)
                {
                    thisFragment.PerformShowFragmentTransaction(request, fragmentHost.ChildFragmentManager, attribute);
                }
                else
                {
                    thisFragment.PerformShowFragmentTransaction(request, base.Context.CurrentFragmentManager, attribute);
                }
            }

            return ValueTask.FromResult(true);
        }
    }
}
