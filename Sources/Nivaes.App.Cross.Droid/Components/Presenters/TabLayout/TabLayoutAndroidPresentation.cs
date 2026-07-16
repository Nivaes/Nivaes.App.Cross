using Android.Content;
using Android.OS;
using Android.Util;
using AndroidX.ViewPager.Widget;
using Google.Android.Material.Tabs;
using Microsoft.Extensions.Logging;
using Activity = AndroidX.AppCompat.App.AppCompatActivity;
using DialogFragment = AndroidX.Fragment.App.DialogFragment;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;
using FragmentTransaction = AndroidX.Fragment.App.FragmentTransaction;

namespace Nivaes.App.Cross.Droid
{
    public sealed class TabLayoutAndroidPresentation
        : ViewPagerFragmentAndroidPresentation<TabLayoutPresentationAttribute>
    {
        public TabLayoutAndroidPresentation(
                PressenterActionContext context,
                ICrossViewsContainer viewsContainer,
                IMvxAndroidCurrentTopActivity androidCurrentTopActivity,
                ILogger<TabLayoutAndroidPresentation> logger)
            : base(context, viewsContainer, androidCurrentTopActivity, logger)
        {
        }

        protected override async ValueTask<bool> ShowAction(Type viewType, TabLayoutPresentationAttribute attribute, CrossViewModelRequest request)
        {
            var showViewPagerFragment = await base.ShowAction(viewType, attribute, request).ConfigureAwait(true);
            if (!showViewPagerFragment)
                return false;

            ViewPager? viewPager = null;
            TabLayout? tabLayout = null;

            // check for a ViewPager inside a Fragment
            if (attribute.FragmentHostViewType != null)
            {
                var fragment = GetFragmentByViewType(attribute.FragmentHostViewType);

                viewPager = fragment?.View?.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                tabLayout = fragment?.View?.FindViewById<TabLayout>(attribute.TabLayoutResourceId);
            }

            // check for a ViewPager inside an Activity
            if (CurrentActivity.IsActivityAlive() && attribute.ActivityHostViewModelType != null)
            {
                viewPager = CurrentActivity?.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                tabLayout = CurrentActivity?.FindViewById<TabLayout>(attribute.TabLayoutResourceId);
            }

            if (viewPager == null || tabLayout == null)
                throw new AppException("ViewPager or TabLayout not found");

            tabLayout.SetupWithViewPager(viewPager);
            return true;
        }
    }
}
