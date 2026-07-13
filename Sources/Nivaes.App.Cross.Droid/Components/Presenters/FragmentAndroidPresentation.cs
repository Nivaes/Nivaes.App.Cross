using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Text;
using Microsoft.Extensions.Logging;
using Activity = AndroidX.AppCompat.App.AppCompatActivity;
using DialogFragment = AndroidX.Fragment.App.DialogFragment;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;
using FragmentTransaction = AndroidX.Fragment.App.FragmentTransaction;

namespace Nivaes.App.Cross.Droid
{
    public sealed class FragmentAndroidPresentation
        : AndroidPressenterAction<MvxFragmentPresentationAttribute>
    {
        protected readonly ICrossNavigationSerializer NavigationSerializer;

        public FragmentAndroidPresentation(
                ICrossViewsContainer viewsContainer,
                IMvxAndroidCurrentTopActivity androidCurrentTopActivity,
                ICrossNavigationSerializer navigationSerializer,
                ILogger<MvxFragmentPresentationAttribute> logger)
            : base(viewsContainer, androidCurrentTopActivity, logger)
        {
            NavigationSerializer = navigationSerializer;
        }

        // ToDo: Poner ICrossPresentationAttribute como generico.
        protected override ValueTask<bool> ShowAction(Type viewType, MvxFragmentPresentationAttribute attribute, CrossViewModelRequest request)
        {
            var fragmentAttribute = (MvxFragmentPresentationAttribute)attribute;
            // if attribute has a Fragment Host, then show it as nested and return
            if (fragmentAttribute.FragmentHostViewType != null)
            {
                ShowNestedFragment(viewType, fragmentAttribute, request);

                return ValueTask.FromResult(true);
            }

            // if there is no Activity host associated, assume is the current activity
            if (fragmentAttribute.ActivityHostViewModelType == null)
                fragmentAttribute.ActivityHostViewModelType = GetCurrentActivityViewModelType();

            var currentHostViewModelType = GetCurrentActivityViewModelType();
            if (fragmentAttribute.ActivityHostViewModelType != currentHostViewModelType)
            {
                Logger.LogWarning("Activity host with ViewModelType {ActivityHostViewModelType} is not CurrentTopActivity. Showing Activity before showing Fragment for {ViewModelType}",
                    fragmentAttribute.ActivityHostViewModelType, attribute.ViewModelType);
                PendingRequest = request;
                ShowHostActivity(attribute);
            }
            else if (CurrentActivity!.IsActivityAlive())
            {
                if (CurrentActivity!.FindViewById(attribute.FragmentContentId) == null)
                    throw new InvalidOperationException("FrameLayout to show Fragment not found");

                PerformShowFragmentTransaction(CurrentActivity.SupportFragmentManager, attribute, request);
            }
            return ValueTask.FromResult(true);
        }

        protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, MvxFragmentPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(attribute);

            // try to close nested fragment first
            if (attribute.FragmentHostViewType != null)
            {
                var fragmentHost = GetFragmentByViewType(attribute.FragmentHostViewType);
                if (fragmentHost != null
                    && TryPerformCloseFragmentTransaction(fragmentHost.ChildFragmentManager, attribute))
                    return ValueTask.FromResult(true);
            }

            // Close fragment. If it isn't successful, then close the current Activity
            if (CurrentFragmentManager != null && TryPerformCloseFragmentTransaction(CurrentFragmentManager, attribute))
            {
                return ValueTask.FromResult(true);
            }

            if (CurrentActivity.IsActivityAlive())
            {
                CurrentActivity!.Finish();
                return ValueTask.FromResult(true);
            }

            return ValueTask.FromResult(false);
        }

        private void ShowNestedFragment(
            Type view,
            MvxFragmentPresentationAttribute attribute,
            CrossViewModelRequest request)
        {
            ArgumentNullException.ThrowIfNull(view);
            ArgumentNullException.ThrowIfNull(attribute);
            ArgumentNullException.ThrowIfNull(request);

            // current implementation only supports one level of nesting 

            var fragmentHost = GetFragmentByViewType(attribute.FragmentHostViewType);
            if (fragmentHost == null)
                throw new InvalidOperationException($"Fragment host not found when trying to show View {view.Name} as Nested Fragment");

            if (!fragmentHost.IsVisible)
                Logger.Log(LogLevel.Warning, "Fragment host is not visible when trying to show View {ViewName} as Nested Fragment", view.Name);

            PerformShowFragmentTransaction(fragmentHost.ChildFragmentManager, attribute, request);
        }

        private void PerformShowFragmentTransaction(
            FragmentManager fragmentManager,
            MvxFragmentPresentationAttribute attribute,
            CrossViewModelRequest request)
        {
            ArgumentNullException.ThrowIfNull(attribute);
            ArgumentNullException.ThrowIfNull(request);

            ArgumentNullException.ThrowIfNull(fragmentManager, nameof(fragmentManager));

            var fragmentName = attribute.Tag ?? attribute.ViewType!.FragmentJavaName();

            IMvxFragmentView? fragmentView = null;
            if (attribute.IsCacheableFragment)
            {
                fragmentView = (IMvxFragmentView?)fragmentManager.FindFragmentByTag(fragmentName);
            }

            if (fragmentView == null && attribute.ViewType != null)
                fragmentView = CreateFragment(fragmentManager, attribute, attribute.ViewType);

            var fragment = fragmentView?.ToFragment();
            if (fragment == null)
                throw new CrossException($"Fragment {fragmentName} is null. Cannot perform Fragment Transaction.");

            // MvxNavigationService provides an already instantiated ViewModel here
            if (request is CrossViewModelInstanceRequest instanceRequest)
            {
                fragmentView!.ViewModel = instanceRequest.ViewModelInstance;
            }

            // save MvxViewModelRequest in the Fragment's Arguments
            var bundle = new Bundle();
            var serializedRequest = NavigationSerializer?.Serializer.SerializeObject(request);
            if (!string.IsNullOrEmpty(serializedRequest))
                bundle.PutString(AndroidViewPresenterManager.ViewModelRequestBundleKey, serializedRequest);

            if (fragment.Arguments == null)
            {
                fragment.Arguments = bundle;
            }
            else
            {
                fragment.Arguments.Clear();
                fragment.Arguments.PutAll(bundle);
            }

            var ft = fragmentManager.BeginTransaction();

            OnBeforeFragmentChanging(ft, fragment, attribute, request);

            ft.SetReorderingAllowed(attribute.AllowReordering);

            if (attribute.AddToBackStack)
                ft.AddToBackStack(fragmentName);

            OnFragmentChanging(ft, fragment, attribute, request);

            if (attribute.AddFragment && fragment.IsAdded)
            {
                ft.Show(fragment);
            }
            else if (attribute.AddFragment)
            {
                ft.Add(attribute.FragmentContentId, fragment, fragmentName);
            }
            else
            {
                ft.Replace(attribute.FragmentContentId, fragment, fragmentName);
            }

            if (attribute.SetAsPrimaryFragment)
                ft.SetPrimaryNavigationFragment(fragment);

            ft.CommitAllowingStateLoss();

            OnFragmentChanged(ft, fragment, attribute, request);
        }

        private bool TryPerformCloseFragmentTransaction(
            FragmentManager fragmentManager,
            MvxFragmentPresentationAttribute fragmentAttribute)
        {
            ArgumentNullException.ThrowIfNull(fragmentAttribute);

            ArgumentNullException.ThrowIfNull(fragmentManager, nameof(fragmentManager));

            try
            {
                var fragmentName = fragmentAttribute.Tag ?? fragmentAttribute.ViewType.FragmentJavaName();
                if (fragmentManager.BackStackEntryCount > 0)
                {
                    PopOnBackstackEntries(fragmentName, fragmentManager, fragmentAttribute);
                    return true;
                }

                Fragment? fragmentToPop = fragmentManager.FindFragmentByTag(fragmentName);
                if (fragmentToPop != null)
                {
                    PopFragment(fragmentManager, fragmentAttribute, fragmentToPop);
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "Cannot close fragment transaction");
                return false;
            }

            return false;
        }

        private void PopFragment(FragmentManager fragmentManager,
                MvxFragmentPresentationAttribute fragmentAttribute,
                Fragment fragmentToPop)
        {
            var ft = fragmentManager.BeginTransaction();

            if (!fragmentAttribute.EnterAnimation.Equals(int.MinValue) &&
                !fragmentAttribute.ExitAnimation.Equals(int.MinValue))
            {
                if (!fragmentAttribute.PopEnterAnimation.Equals(int.MinValue) &&
                    !fragmentAttribute.PopExitAnimation.Equals(int.MinValue))
                {
                    ft.SetCustomAnimations(
                        fragmentAttribute.EnterAnimation,
                        fragmentAttribute.ExitAnimation,
                        fragmentAttribute.PopEnterAnimation,
                        fragmentAttribute.PopExitAnimation);
                }
                else
                {
                    ft.SetCustomAnimations(
                        fragmentAttribute.EnterAnimation,
                        fragmentAttribute.ExitAnimation);
                }
            }

            if (fragmentAttribute.TransitionStyle != int.MinValue)
                ft.SetTransitionStyle(fragmentAttribute.TransitionStyle);

            ft.Remove(fragmentToPop);
            ft.CommitAllowingStateLoss();

            OnFragmentPopped(ft, fragmentToPop, fragmentAttribute);
        }

        private void PopOnBackstackEntries(
                string fragmentName, FragmentManager fragmentManager, MvxFragmentPresentationAttribute fragmentAttribute)
        {
            var popBackStackFragmentName =
                string.IsNullOrEmpty(fragmentAttribute.PopBackStackImmediateName.Trim())
                    ? fragmentName
                    : fragmentAttribute.PopBackStackImmediateName;

            fragmentManager.PopBackStackImmediate(
                popBackStackFragmentName,
                (int)fragmentAttribute.PopBackStackImmediateFlag.ToNativePopBackStackFlags());

            OnFragmentPopped(null, null, fragmentAttribute);
        }
    }
}
