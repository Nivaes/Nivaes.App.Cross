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
        : ViewPagerFragmentAndroidPresentation<MvxTabLayoutPresentationAttribute>
    {
        public TabLayoutAndroidPresentation(
                ICrossViewsContainer viewsContainer,
                IMvxAndroidCurrentTopActivity androidCurrentTopActivity,
                ICrossNavigationSerializer navigationSerializer,
                ILogger<TabLayoutAndroidPresentation> logger)
            : base(viewsContainer, androidCurrentTopActivity, navigationSerializer, logger)
        {
        }

        protected override async Task<bool> ShowAction(Type view, MvxTabLayoutPresentationAttribute attribute, CrossViewModelRequest request)
        {
            ArgumentNullException.ThrowIfNull(view);
            ArgumentNullException.ThrowIfNull(attribute);
            ArgumentNullException.ThrowIfNull(request);

            var showViewPagerFragment = await base.ShowAction(view, attribute, request).ConfigureAwait(true);
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
                throw new CrossException("ViewPager or TabLayout not found");

            tabLayout.SetupWithViewPager(viewPager);
            return true;
        }
    }
}
