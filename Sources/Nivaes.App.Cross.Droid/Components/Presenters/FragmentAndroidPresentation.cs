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
        public FragmentAndroidPresentation(
                ICrossViewsContainer viewsContainer,
                IMvxAndroidCurrentTopActivity androidCurrentTopActivity,
                ICrossNavigationSerializer navigationSerializer,
                //IMvxAndroidViewModelRequestTranslator viewModelRequestTranslator,
                ILogger<MvxFragmentPresentationAttribute> logger)
            : base(viewsContainer, androidCurrentTopActivity, navigationSerializer, logger)
        {
        }

        // ToDo: Poner ICrossPresentationAttribute como generico.
        protected override Task<bool> ShowAction(Type view, MvxFragmentPresentationAttribute attribute, CrossViewModelRequest request)
        {
            ArgumentNullException.ThrowIfNull(view);
            ArgumentNullException.ThrowIfNull(attribute);
            ArgumentNullException.ThrowIfNull(request);

            var fragmentAttribute = (MvxFragmentPresentationAttribute)attribute;
            // if attribute has a Fragment Host, then show it as nested and return
            if (fragmentAttribute.FragmentHostViewType != null)
            {
                ShowNestedFragment(view, fragmentAttribute, request);

                return Task.FromResult(true);
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
            return Task.FromResult(true);
        }

        protected override Task<bool> CloseAction(ICrossViewModel viewModel, MvxFragmentPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(attribute);

            // try to close nested fragment first
            if (attribute.FragmentHostViewType != null)
            {
                var fragmentHost = GetFragmentByViewType(attribute.FragmentHostViewType);
                if (fragmentHost != null
                    && TryPerformCloseFragmentTransaction(fragmentHost.ChildFragmentManager, attribute))
                    return Task.FromResult(true);
            }

            // Close fragment. If it isn't successful, then close the current Activity
            if (CurrentFragmentManager != null && TryPerformCloseFragmentTransaction(CurrentFragmentManager, attribute))
            {
                return Task.FromResult(true);
            }

            if (CurrentActivity.IsActivityAlive())
            {
                CurrentActivity!.Finish();
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
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
    }
}
