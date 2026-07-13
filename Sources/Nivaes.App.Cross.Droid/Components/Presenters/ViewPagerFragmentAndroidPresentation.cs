using Android.Content;
using Android.OS;
using Android.Util;
using AndroidX.ViewPager.Widget;
using Java.Util.Logging;
using Microsoft.Extensions.Logging;
using Activity = AndroidX.AppCompat.App.AppCompatActivity;
using DialogFragment = AndroidX.Fragment.App.DialogFragment;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;
using FragmentTransaction = AndroidX.Fragment.App.FragmentTransaction;

namespace Nivaes.App.Cross.Droid
{
    public abstract class ViewPagerFragmentAndroidPresentation<ViewPagerFragmentPresentationAttribute>
        : AndroidPressenterAction<ViewPagerFragmentPresentationAttribute>
        where ViewPagerFragmentPresentationAttribute : MvxViewPagerFragmentPresentationAttribute
    {
        public ViewPagerFragmentAndroidPresentation(
                ICrossViewsContainer viewsContainer,
                IMvxAndroidCurrentTopActivity androidCurrentTopActivity,
                ILogger logger)
            : base(viewsContainer, androidCurrentTopActivity, logger)
        { }

        protected override Task<bool> ShowAction(Type view, ViewPagerFragmentPresentationAttribute attribute, CrossViewModelRequest request)
        {
            ArgumentNullException.ThrowIfNull(view);
            ArgumentNullException.ThrowIfNull(attribute);
            ArgumentNullException.ThrowIfNull(request);

            // if the attribute doesn't supply any host, assume current activity!
            if (attribute.FragmentHostViewType == null && attribute.ActivityHostViewModelType == null)
                attribute.ActivityHostViewModelType = GetCurrentActivityViewModelType();

            ViewPager? viewPager = null;
            FragmentManager? fragmentManager = null;

            // check for a ViewPager inside a Fragment
            if (attribute.FragmentHostViewType != null)
            {
                var fragment = GetFragmentByViewType(attribute.FragmentHostViewType);
                if (fragment == null)
                    throw new CrossException("Fragment not found", attribute.FragmentHostViewType.Name);

                if (fragment.View == null)
                {
                    throw new CrossException("Fragment.View is null. Please consider calling Navigate later in your code",
                        attribute.FragmentHostViewType.Name);
                }

                viewPager = fragment.View.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                fragmentManager = fragment.ChildFragmentManager;
            }

            // check for a ViewPager inside an Activity
            if (attribute.ActivityHostViewModelType != null)
            {
                var currentActivityViewModelType = GetCurrentActivityViewModelType();

                // if the host Activity is not the top-most Activity, then show it before proceeding, and return false for now
                if (attribute.ActivityHostViewModelType != currentActivityViewModelType)
                {
                    PendingRequest = request;
                    ShowHostActivity(attribute);
                    return Task.FromResult(false);
                }

                if (CurrentActivity.IsActivityAlive())
                    viewPager = CurrentActivity!.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                fragmentManager = CurrentFragmentManager;
            }

            // no more cases to check. Just throw if ViewPager wasn't found
            if (viewPager == null)
                throw new CrossException("ViewPager not found");

            var tag = attribute.Tag ?? attribute.ViewType?.FragmentJavaName();
            var fragmentInfo = new MvxViewPagerFragmentInfo(attribute.Title, tag, attribute.ViewType, request);

            if (viewPager.Adapter is MvxCachingFragmentStatePagerAdapter adapter)
            {
                adapter.FragmentsInfo.Add(fragmentInfo);
                adapter.NotifyDataSetChanged();
            }
            else
            {
                viewPager.Adapter = new MvxCachingFragmentStatePagerAdapter(
                    fragmentManager,
                    new List<MvxViewPagerFragmentInfo>
                    {
                    fragmentInfo
                    }
                );
            }

            return Task.FromResult(true);
        }

        protected override Task<bool> CloseAction(ICrossViewModel viewModel, ViewPagerFragmentPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(attribute);

            ViewPager? viewPager = null;
            FragmentManager? fragmentManager;

            if (attribute.FragmentHostViewType != null)
            {
                var fragment = GetFragmentByViewType(attribute.FragmentHostViewType);
                if (fragment == null)
                    throw new CrossException("Fragment not found", attribute.FragmentHostViewType.Name);

                viewPager = fragment.View?.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                fragmentManager = fragment.ChildFragmentManager;
            }
            else
            {
                if (CurrentActivity.IsActivityAlive())
                    viewPager = CurrentActivity!.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                fragmentManager = CurrentFragmentManager;
            }

            if (viewPager?.Adapter is MvxCachingFragmentStatePagerAdapter adapter && fragmentManager != null)
            {
                var ft = fragmentManager.BeginTransaction();
                var fragmentInfo = FindFragmentInfoFromAttribute(attribute, adapter);
                if (fragmentInfo != null)
                {
                    var fragment = fragmentManager.FindFragmentByTag(fragmentInfo.Tag);
                    adapter.FragmentsInfo.Remove(fragmentInfo);

                    if (fragment != null)
                        ft.Remove(fragment);

                    ft.CommitAllowingStateLoss();
                    adapter.NotifyDataSetChanged();

                    OnFragmentPopped(ft, fragment, attribute);
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }

        private MvxViewPagerFragmentInfo? FindFragmentInfoFromAttribute(
           MvxFragmentPresentationAttribute attribute,
           MvxCachingFragmentStatePagerAdapter adapter)
        {
            ArgumentNullException.ThrowIfNull(attribute);
            ArgumentNullException.ThrowIfNull(adapter);

            MvxViewPagerFragmentInfo? fragmentInfo = null;
            if (attribute.Tag != null)
            {
                fragmentInfo = adapter.FragmentsInfo?.Find(f => f.Tag == attribute.Tag);
            }

            if (fragmentInfo != null)
                return fragmentInfo;

            fragmentInfo = adapter.FragmentsInfo?.Find(IsMatch);
            return fragmentInfo;

            bool IsMatch(MvxViewPagerFragmentInfo? info)
            {
                if (attribute.ViewType == null) return false;

                var viewTypeMatches = info?.FragmentType == attribute.ViewType;

                if (attribute.ViewModelType != null)
                    return viewTypeMatches && info?.Request?.ViewModelType == attribute.ViewModelType;

                return viewTypeMatches;
            }
        }
    }

    public sealed class ViewPagerFragmentAndroidPresentation
        : ViewPagerFragmentAndroidPresentation<MvxViewPagerFragmentPresentationAttribute>
    {
        public ViewPagerFragmentAndroidPresentation(
                ICrossViewsContainer viewsContainer,
                IMvxAndroidCurrentTopActivity androidCurrentTopActivity,
                ILogger<TabLayoutAndroidPresentation> logger)
            : base(viewsContainer, androidCurrentTopActivity, logger)
        {
        }
    }
}
