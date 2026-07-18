using Microsoft.Extensions.Logging;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace Nivaes.App.Cross.Droid
{
    public sealed class FullPresentationAction
        : ViewPagerFragmentPresentationAction<FullPresentationAttribute>, IFragmentPresentationAction
    {
        private IFragmentPresentationAction thisFragment => (IFragmentPresentationAction)this;

        protected readonly ICrossNavigationSerializer NavigationSerializer;

        public FullPresentationAction(
                IPressenterActionContext context,
                ICrossViewsContainer viewsContainer,
                ICrossNavigationSerializer navigationSerializer,
                ILogger<FullPresentationAction> logger)
            : base(context, viewsContainer, navigationSerializer, logger)
        {
            NavigationSerializer = navigationSerializer;
        }

        // ToDo: Poner ICrossPresentationAttribute como generico.
        protected override ValueTask<bool> ShowAction(Type viewType, FullPresentationAttribute attribute, CrossViewModelRequest request)
        {
            if (attribute.FragmentHostViewType != null)
            {
                thisFragment.ShowNestedFragment(viewType, attribute, request);

                return ValueTask.FromResult(true);
            }

            if (attribute.ActivityHostViewModelType == null)
                attribute.ActivityHostViewModelType = GetCurrentActivityViewModelType();

            var currentHostViewModelType = GetCurrentActivityViewModelType();
            if (attribute.ActivityHostViewModelType != currentHostViewModelType)
            {
                Logger.LogTrace("Activity host with ViewModelType {0} is not CurrentTopActivity. Showing Activity before showing Fragment for {1}",
                    attribute.ActivityHostViewModelType, attribute.ViewModelType);

                base.PendingRequest = request;
                base.ShowHostActivity(attribute);
            }
            else
            {
                Fragment fragmentHost;
                if (attribute.FragmentHostFullView != null &&
                    (fragmentHost = thisFragment.GetFragmentByViewType(attribute.FragmentHostFullView)) != null)
                {
                    thisFragment.PerformShowFragmentTransaction(fragmentHost.ChildFragmentManager, attribute, request);
                }
                else
                {
                    thisFragment.PerformShowFragmentTransaction(base.Context.CurrentFragmentManager, attribute, request);
                }
            }

            return ValueTask.FromResult(true);
        }
    }
}
