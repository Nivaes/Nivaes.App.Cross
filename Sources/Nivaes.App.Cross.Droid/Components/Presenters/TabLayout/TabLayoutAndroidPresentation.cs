using AndroidX.ViewPager.Widget;
using Google.Android.Material.Tabs;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Droid
{
    public sealed class TabLayoutAndroidPresentation
        : ViewPagerFragmentPresentationAction<TabLayoutPresentationAttribute>, IFragmentPresentationAction
    {
        private IFragmentPresentationAction thisFragment => (IFragmentPresentationAction)this;

        public TabLayoutAndroidPresentation(
                IPressenterActionContext context,
                ILogger<TabLayoutAndroidPresentation> logger)
            : base(context, logger)
        {
        }

        protected override async ValueTask<bool> ShowAction(TabLayoutPresentationAttribute attribute, IViewModelRequest request)
        {
            var showViewPagerFragment = await ShowAction(attribute, request).ConfigureAwait(true);
            if (!showViewPagerFragment)
                return false;

            ViewPager? viewPager = null;
            TabLayout? tabLayout = null;

            // check for a ViewPager inside a Fragment
            if (attribute.FragmentHostViewType != null)
            {
                var fragment = thisFragment.GetFragmentByViewType(attribute.FragmentHostViewType);

                viewPager = fragment?.View?.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                tabLayout = fragment?.View?.FindViewById<TabLayout>(attribute.TabLayoutResourceId);
            }

            // check for a ViewPager inside an Activity
            if (base.Context.CurrentActivity.IsActivityAlive() && attribute.ActivityHostViewModelType != null)
            {
                viewPager = base.Context.CurrentActivity?.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                tabLayout = base.Context.CurrentActivity?.FindViewById<TabLayout>(attribute.TabLayoutResourceId);
            }

            if (viewPager == null || tabLayout == null)
                throw new AppException("ViewPager or TabLayout not found");

            tabLayout.SetupWithViewPager(viewPager);
            return true;
        }
    }
}
