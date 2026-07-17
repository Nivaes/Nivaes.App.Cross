using AndroidX.ViewPager.Widget;
using Microsoft.Extensions.Logging;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;

namespace Nivaes.App.Cross.Droid
{
    public abstract class ViewPagerFragmentPresentationAction<TViewPagerFragmentPresentationAttribute>
        : PressenterAction<TViewPagerFragmentPresentationAttribute>
        where TViewPagerFragmentPresentationAttribute : ViewPagerFragmentPresentationAttribute
    {
        private IFragmentPresentationAction thisFragment => (IFragmentPresentationAction)this;

        public ViewPagerFragmentPresentationAction(
                PressenterActionContext context,
                ICrossViewsContainer viewsContainer,
                ICrossNavigationSerializer navigationSerializer,
                ILogger logger)
            : base(context, viewsContainer, navigationSerializer, logger)
        { }

        protected override ValueTask<bool> ShowAction(Type viewType, TViewPagerFragmentPresentationAttribute attribute, CrossViewModelRequest request)
        {
            // if the attribute doesn't supply any host, assume current activity!
            if (attribute.FragmentHostViewType == null && attribute.ActivityHostViewModelType == null)
                attribute.ActivityHostViewModelType = GetCurrentActivityViewModelType();

            ViewPager? viewPager = null;
            FragmentManager? fragmentManager = null;

            // check for a ViewPager inside a Fragment
            if (attribute.FragmentHostViewType != null)
            {
                var fragment = thisFragment.GetFragmentByViewType(attribute.FragmentHostViewType);
                if (fragment == null)
                    throw new AppException("Fragment not found", attribute.FragmentHostViewType.Name);

                if (fragment.View == null)
                {
                    throw new AppException("Fragment.View is null. Please consider calling Navigate later in your code",
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
                    return ValueTask.FromResult(false);
                }

                if (base.Context.CurrentActivity.IsActivityAlive())
                    viewPager = base.Context.CurrentActivity!.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                fragmentManager = base.Context.CurrentFragmentManager;
            }

            // no more cases to check. Just throw if ViewPager wasn't found
            if (viewPager == null)
                throw new AppException("ViewPager not found");

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

            return ValueTask.FromResult(true);
        }

        protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, TViewPagerFragmentPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(attribute);

            ViewPager? viewPager = null;
            FragmentManager? fragmentManager;

            if (attribute.FragmentHostViewType != null)
            {
                var fragment = thisFragment.GetFragmentByViewType(attribute.FragmentHostViewType);
                if (fragment == null)
                    throw new AppException("Fragment not found", attribute.FragmentHostViewType.Name);

                viewPager = fragment.View?.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                fragmentManager = fragment.ChildFragmentManager;
            }
            else
            {
                if (base.Context.CurrentActivity.IsActivityAlive())
                    viewPager = base.Context.CurrentActivity!.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                fragmentManager = base.Context.CurrentFragmentManager;
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

                    thisFragment.OnFragmentPopped(ft, fragment, attribute);
                    return ValueTask.FromResult(true);
                }
            }

            return ValueTask.FromResult(false);
        }

        private MvxViewPagerFragmentInfo? FindFragmentInfoFromAttribute(
           FragmentPresentationAttribute attribute,
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

    public sealed class ViewPagerFragmentPresentationAction
        : ViewPagerFragmentPresentationAction<ViewPagerFragmentPresentationAttribute>
    {
        public ViewPagerFragmentPresentationAction(
                PressenterActionContext context,
                ICrossViewsContainer viewsContainer,
                ICrossNavigationSerializer navigationSerializer,
                ILogger<TabLayoutAndroidPresentation> logger)
            : base(context, viewsContainer, navigationSerializer, logger)
        {
        }
    }
}
