﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Google.Android.Material.Tabs;

namespace Nivaes.App.Cross.Presenters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Android.Views;
    using AndroidX.ViewPager.Widget;
    using Java.Lang;
    using MvvmCross.Exceptions;
    using MvvmCross.Platforms.Android;
    using MvvmCross.Platforms.Android.Views;
    using MvvmCross.Platforms.Android.Views.Fragments;
    using MvvmCross.Platforms.Android.Views.ViewPager;
    using MvvmCross.Views;
    using Nivaes.App.Cross.Droid;
    using Nivaes.App.Cross.ViewModels;
    using Activity = AndroidX.AppCompat.App.AppCompatActivity;
    using DialogFragment = AndroidX.Fragment.App.DialogFragment;
    using Fragment = AndroidX.Fragment.App.Fragment;
    using FragmentManager = AndroidX.Fragment.App.FragmentManager;
    using FragmentTransaction = AndroidX.Fragment.App.FragmentTransaction;

    public class MvxAndroidViewPresenter
        : MvxAttributeViewPresenter, IMvxAndroidViewPresenter
    {
        private readonly Lazy<IMvxAndroidCurrentTopActivity> mAndroidCurrentTopActivity =
            new Lazy<IMvxAndroidCurrentTopActivity>(() => Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>());

        private readonly Lazy<IMvxAndroidActivityLifetimeListener> mActivityLifetimeListener =
            new Lazy<IMvxAndroidActivityLifetimeListener>(() => Mvx.IoCProvider.Resolve<IMvxAndroidActivityLifetimeListener>());

        private readonly Lazy<IMvxNavigationSerializer> mNavigationSerializer =
            new Lazy<IMvxNavigationSerializer>(() => Mvx.IoCProvider.Resolve<IMvxNavigationSerializer>());

        [Obsolete("Not user reflector")]
        protected IEnumerable<Assembly> AndroidViewAssemblies { get; set; }
        public const string ViewModelRequestBundleKey = "__viewModelRequest";
        public const string SharedElementsBundleKey = "__sharedElementsKey";

        protected MvxViewModelRequest? PendingRequest { get; set; }

        protected virtual FragmentManager? CurrentFragmentManager
        {
            get
            {
                if (CurrentActivity.IsActivityDead())
                    return null;

                return CurrentActivity!.SupportFragmentManager;
            }
        }
        protected virtual Activity? CurrentActivity
        {
            get
            {
                return mAndroidCurrentTopActivity.Value?.Activity as Activity;
            }
        }

        protected IMvxAndroidActivityLifetimeListener ActivityLifetimeListener => mActivityLifetimeListener.Value;

        protected IMvxNavigationSerializer NavigationSerializer => mNavigationSerializer.Value;

        [Obsolete("Not user reflector")]
        public MvxAndroidViewPresenter(IEnumerable<Assembly> androidViewAssemblies)
        {
            AndroidViewAssemblies = androidViewAssemblies;
            ActivityLifetimeListener.ActivityChanged += ActivityLifetimeListenerActivityChanged;
        }

        protected virtual void ActivityLifetimeListenerActivityChanged(object sender, MvxActivityEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            if (e.ActivityState == MvxActivityState.OnResume && PendingRequest != null)
            {
                _ = Show(PendingRequest).AsTask();
                PendingRequest = null;
            }
            else if (e.ActivityState == MvxActivityState.OnCreate && e.Extras is Bundle savedBundle)
            {
                //TODO: Restore fragments from bundle
            }
            else if (e.ActivityState == MvxActivityState.OnSaveInstanceState && e.Extras is Bundle outBundle)
            {
                //TODO: Save fragments into bundle
            }
            else if (e.ActivityState == MvxActivityState.OnDestroy)
            {
                //TODO: Should be check for Fragments on this Activity and destroy them?
            }
        }

        protected Type GetAssociatedViewModelType(Type fromFragmentType)
        {
            Type viewModelType = ViewModelTypeFinder.FindTypeOrNull(fromFragmentType);

            return viewModelType ?? fromFragmentType.GetBasePresentationAttributes().First().ViewModelType;
        }

        public override void RegisterAttributeTypes()
        {
            AttributeTypesToActionsDictionary.Register<MvxActivityPresentationAttribute>(ShowActivity, CloseActivity);
            AttributeTypesToActionsDictionary.Register<MvxFragmentPresentationAttribute>(ShowFragment, CloseFragment);
            AttributeTypesToActionsDictionary.Register<MvxDialogFragmentPresentationAttribute>(ShowDialogFragment, CloseFragmentDialog);

            AttributeTypesToActionsDictionary.Register<MvxTabLayoutPresentationAttribute>(ShowTabLayout, CloseViewPagerFragment);
            AttributeTypesToActionsDictionary.Register<MvxViewPagerFragmentPresentationAttribute>(ShowViewPagerFragment, CloseViewPagerFragment);
        }

        public override ValueTask<MvxBasePresentationAttribute?> GetPresentationAttribute(MvxViewModelRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var viewType = ViewsContainer.GetViewType(request.ViewModelType);

            var overrideAttribute = GetOverridePresentationAttribute(request, viewType);
            if (overrideAttribute != null)
                return overrideAttribute;

            var attributes = viewType.GetCustomAttributes<MvxBasePresentationAttribute>(true);
            if (attributes != null && attributes.Any())
            {
                MvxBasePresentationAttribute? attribute = null;

                if (attributes.Count() > 1)
                {
                    var fragmentAttributes = attributes.OfType<MvxFragmentPresentationAttribute>();

                    // check if fragment can be displayed as child fragment first
                    foreach (var item in fragmentAttributes.Where(att => att.FragmentHostViewType != null))
                    {
                        var fragment = GetFragmentByViewType(item.FragmentHostViewType!);

                        // if the fragment exists, and is on top, then use the current attribute 
                        if (fragment?.IsVisible == true && fragment.View.FindViewById(item.FragmentContentId) != null)
                        {
                            attribute = item;
                            break;
                        }
                    }

                    // if attribute is still null, check if fragment can be displayed in current activity
                    if (attribute == null && CurrentActivity != null)
                    {
                        var currentActivityHostViewModelType = GetCurrentActivityViewModelType();
                        foreach (var item in fragmentAttributes.Where(att => att.ActivityHostViewModelType != null))
                        {
                            if (CurrentActivity.FindViewById(item.FragmentContentId) != null && item.ActivityHostViewModelType == currentActivityHostViewModelType)
                            {
                                attribute = item;
                                break;
                            }
                        }
                    }
                }

                if (attribute == null)
                    attribute = attributes.FirstOrDefault();

                attribute.ViewType = viewType;

                return new ValueTask<MvxBasePresentationAttribute?>(attribute);
            }

            return CreatePresentationAttribute(request.ViewModelType, viewType);
        }

        public override ValueTask<MvxBasePresentationAttribute?> CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            if (viewType == null) throw new ArgumentNullException(nameof(viewType));

            if (viewType.IsSubclassOf(typeof(DialogFragment)))
            {
                //MvxLog.Instance?.Trace("PresentationAttribute not found for {0}. Assuming DialogFragment presentation", viewType.Name);
                return new ValueTask<MvxBasePresentationAttribute?>(new MvxDialogFragmentPresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType });
            }
            else if (viewType.IsSubclassOf(typeof(Fragment)))
            {
                //MvxLog.Instance?.Trace("PresentationAttribute not found for {0}. Assuming Fragment presentation", viewType.Name);
                return new ValueTask<MvxBasePresentationAttribute?>(new MvxFragmentPresentationAttribute(GetCurrentActivityViewModelType(), global::Android.Resource.Id.Content) { ViewType = viewType, ViewModelType = viewModelType });
            }
            else if (viewType.IsSubclassOf(typeof(Activity)))
            {
                //MvxLog.Instance?.Trace("PresentationAttribute not found for {0}. Assuming Activity presentation", viewType.Name);
                return new ValueTask<MvxBasePresentationAttribute?>(new MvxActivityPresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType });
            }
            return new ValueTask<MvxBasePresentationAttribute?>((MvxBasePresentationAttribute)null!);
        }

        public override async ValueTask<bool> ChangePresentation(MvxPresentationHint? hint)
        {
            if (hint is MvxPagePresentationHint pagePresentationHint)
            {
                var request = new MvxViewModelRequest(pagePresentationHint.ViewModel);
                var attribute = await GetPresentationAttribute(request).ConfigureAwait(false);

                if (attribute is MvxViewPagerFragmentPresentationAttribute pagerFragmentAttribute)
                {
                    var viewPager = FindViewPagerInFragmentPresentation(pagerFragmentAttribute);
                    if (viewPager?.Adapter is MvxCachingFragmentStatePagerAdapter adapter)
                    {
                        var fragmentInfo = FindFragmentInfoFromAttribute(pagerFragmentAttribute, adapter);
                        var index = adapter.FragmentsInfo.IndexOf(fragmentInfo);
                        if (index < 0)
                        {
                            //MvxLog.Instance?.Trace("Did not find ViewPager index for {0}, skipping presentation change...",
                            //    pagerFragmentAttribute.Tag);

                            return false;
                        }

                        viewPager.SetCurrentItem(index, true);
                        return true;
                    }
                }
            }

            return await base.ChangePresentation(hint);
        }

        protected virtual ViewPager? FindViewPagerInFragmentPresentation(
            MvxViewPagerFragmentPresentationAttribute? pagerFragmentAttribute)
        {
            if (pagerFragmentAttribute == null)
                throw new ArgumentNullException(nameof(pagerFragmentAttribute));

            ViewPager? viewPager = null;

            // check for a ViewPager inside a Fragment
            if (pagerFragmentAttribute.FragmentHostViewType != null)
            {
                var fragment = GetFragmentByViewType(pagerFragmentAttribute.FragmentHostViewType);
                viewPager = fragment?.View.FindViewById<ViewPager>(pagerFragmentAttribute.ViewPagerResourceId);
            }

            // check for a ViewPager inside an Activity
            if (viewPager == null && pagerFragmentAttribute.ActivityHostViewModelType != null &&
                CurrentActivity.IsActivityAlive())
            {
                viewPager = CurrentActivity!.FindViewById<ViewPager>(pagerFragmentAttribute.ViewPagerResourceId);
            }

            return viewPager;
        }

        protected Type GetCurrentActivityViewModelType()
        {
            var currentActivityType = CurrentActivity?.GetType();

            var activityViewModelType = ViewModelTypeFinder.FindTypeOrNull(currentActivityType);
            return activityViewModelType;
        }

        #region Show implementations
        protected virtual ValueTask<bool> ShowActivity(
            Type view,
            MvxActivityPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var intent = CreateIntentForRequest(request);
            if (attribute!.Extras != null)
                intent.PutExtras(attribute.Extras);

            ShowIntent(intent, CreateActivityTransitionOptions(intent, attribute, request));
            return new ValueTask<bool>(true);
        }

        protected virtual Bundle CreateActivityTransitionOptions(Intent intent, MvxActivityPresentationAttribute attribute, MvxViewModelRequest request)
        {
            var bundle = Bundle.Empty;

            if (CurrentActivity is IMvxAndroidSharedElements sharedElementsActivity)
            {
                var elements = new List<string>();
                var transitionElementPairs = new List<Pair>();

                foreach (KeyValuePair<string, View> item in sharedElementsActivity.FetchSharedElementsToAnimate(attribute, request))
                {
                    var transitionName = item.Value.GetTransitionNameSupport();
                    if (!string.IsNullOrEmpty(transitionName))
                    {
                        transitionElementPairs.Add(Pair.Create(item.Value, transitionName));
                        elements.Add($"{item.Key}:{transitionName}");
                    }
                    //else
                    //{
                    //    MvxLog.Instance?.Warn("A XML transitionName is required in order to transition a control when navigating.");
                    //}
                }

                if (!transitionElementPairs.Any())
                {
                    //MvxLog.Instance?.Warn("No transition elements are provided");
                    return bundle;
                }

                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    var activityOptions = ActivityOptions.MakeSceneTransitionAnimation(CurrentActivity, transitionElementPairs.ToArray());
                    intent.PutExtra(SharedElementsBundleKey, string.Join("|", elements));
                    bundle = activityOptions.ToBundle();
                }
                //else
                //{
                //    MvxLog.Instance?.Warn("Shared element transition requires Android v21+.");
                //}
            }

            return bundle;
        }

        protected virtual Intent CreateIntentForRequest(MvxViewModelRequest request)
        {
            var requestTranslator = Mvx.IoCProvider.Resolve<IMvxAndroidViewModelRequestTranslator>();

            if (!(request is MvxViewModelInstanceRequest viewModelInstanceRequest))
            {
                return requestTranslator.GetIntentFor(request);
            }

            var intentWithKey = requestTranslator.GetIntentWithKeyFor(
                viewModelInstanceRequest.ViewModelInstance,
                viewModelInstanceRequest
            );

            return intentWithKey.intent;
        }

        protected virtual void ShowIntent(Intent intent, Bundle bundle)
        {
            var activity = CurrentActivity;
            if (activity == null)
            {
                //MvxLog.Instance?.Warn("Cannot Resolve current top activity. Creating new activity from Application Context");
                intent.AddFlags(ActivityFlags.NewTask);
                Application.Context.StartActivity(intent, bundle);
                return;
            }
            activity.StartActivity(intent, bundle);
        }

        protected virtual void ShowHostActivity(MvxFragmentPresentationAttribute attribute)
        {
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));

            var viewType = ViewsContainer.GetViewType(attribute.ActivityHostViewModelType);
            if (viewType == null || !viewType.IsSubclassOf(typeof(Activity)))
                throw new MvxException("The host activity doesn't inherit Activity");

            var hostViewModelRequest = MvxViewModelRequest.GetDefaultRequest(attribute.ActivityHostViewModelType);
            hostViewModelRequest.PresentationValues = PendingRequest?.PresentationValues;
            _ = Show(hostViewModelRequest).AsTask();
        }

        protected virtual ValueTask<bool> ShowFragment(
            Type view,
            MvxFragmentPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));

            // if attribute has a Fragment Host, then show it as nested and return
            if (attribute.FragmentHostViewType != null)
            {
                ShowNestedFragment(view, attribute, request);

                return new ValueTask<bool>(true);
            }

            // if there is no Activity host associated, assume is the current activity
            if (attribute.ActivityHostViewModelType == null)
                attribute.ActivityHostViewModelType = GetCurrentActivityViewModelType();

            var currentHostViewModelType = GetCurrentActivityViewModelType();
            if (attribute.ActivityHostViewModelType != currentHostViewModelType)
            {
                //MvxLog.Instance?.Trace("Activity host with ViewModelType {0} is not CurrentTopActivity. Showing Activity before showing Fragment for {1}",
                //    attribute.ActivityHostViewModelType, attribute.ViewModelType);
                PendingRequest = request;
                ShowHostActivity(attribute);
            }
            else
            {
                if (CurrentActivity?.FindViewById(attribute.FragmentContentId) == null)
                    throw new NullReferenceException("FrameLayout to show Fragment not found");

                PerformShowFragmentTransaction(CurrentActivity.SupportFragmentManager, attribute, request);
            }
            return new ValueTask<bool>(true);
        }

        protected virtual void ShowNestedFragment(
            Type view,
            MvxFragmentPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            // current implementation only supports one level of nesting 

            var fragmentHost = GetFragmentByViewType(attribute.FragmentHostViewType);
            if (fragmentHost == null)
                throw new NullReferenceException($"Fragment host not found when trying to show View {view.Name} as Nested Fragment");

            //if (!fragmentHost.IsVisible)
            //    MvxLog.Instance?.Warn("Fragment host is not visible when trying to show View {0} as Nested Fragment", view.Name);

            PerformShowFragmentTransaction(fragmentHost.ChildFragmentManager, attribute, request);
        }

        protected virtual void PerformShowFragmentTransaction(
            FragmentManager? fragmentManager,
            MvxFragmentPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));

            var fragmentName = attribute.ViewType.FragmentJavaName();

            IMvxFragmentView? fragment = null;
            if (attribute.IsCacheableFragment)
            {
                fragment = (IMvxFragmentView)fragmentManager?.FindFragmentByTag(fragmentName);
            }
            fragment = fragment ?? CreateFragment(fragmentManager, attribute, attribute.ViewType);

            var fragmentView = fragment.ToFragment();

            // MvxNavigationService provides an already instantiated ViewModel here
            if (request is MvxViewModelInstanceRequest instanceRequest)
            {
                fragment.ViewModel = instanceRequest.ViewModelInstance;
            }

            // save MvxViewModelRequest in the Fragment's Arguments
            using var bundle = new Bundle();
            var serializedRequest = NavigationSerializer.Serializer.SerializeObject(request);
            bundle.PutString(ViewModelRequestBundleKey, serializedRequest);

            if (fragmentView != null)
            {
                if (fragmentView.Arguments == null)
                {
                    fragmentView.Arguments = bundle;
                }
                else
                {
                    fragmentView.Arguments.Clear();
                    fragmentView.Arguments.PutAll(bundle);
                }
            }

            var ft = fragmentManager.BeginTransaction();

            OnBeforeFragmentChanging(ft, fragmentView, attribute, request);

            if (attribute.AddToBackStack)
                ft.AddToBackStack(fragmentName);

            OnFragmentChanging(ft, fragmentView, attribute, request);

            ft.Replace(attribute.FragmentContentId, fragmentView, fragmentName);
            ft.CommitAllowingStateLoss();

            OnFragmentChanged(ft, fragmentView, attribute, request);
        }

        protected virtual void OnBeforeFragmentChanging(FragmentTransaction fragmentTransaction, Fragment fragment, MvxFragmentPresentationAttribute attribute, MvxViewModelRequest request)
        {
            if (CurrentActivity is IMvxAndroidSharedElements sharedElementsActivity)
            {
                var elements = new List<string>();

                foreach (KeyValuePair<string, View> item in sharedElementsActivity.FetchSharedElementsToAnimate(attribute, request))
                {
                    var transitionName = item.Value.GetTransitionNameSupport();
                    if (!string.IsNullOrEmpty(transitionName))
                    {
                        fragmentTransaction.AddSharedElement(item.Value, transitionName);
                        elements.Add($"{item.Key}:{transitionName}");
                    }
                    //else
                    //{
                    //    MvxLog.Instance?.Warn("A XML transitionName is required in order to transition a control when navigating.");
                    //}
                }

                if (elements.Count > 0)
                    fragment.Arguments.PutString(SharedElementsBundleKey, string.Join("|", elements));
            }

            if (!attribute.EnterAnimation.Equals(int.MinValue) && !attribute.ExitAnimation.Equals(int.MinValue))
            {
                if (!attribute.PopEnterAnimation.Equals(int.MinValue) && !attribute.PopExitAnimation.Equals(int.MinValue))
                    fragmentTransaction.SetCustomAnimations(attribute.EnterAnimation, attribute.ExitAnimation, attribute.PopEnterAnimation, attribute.PopExitAnimation);
                else
                    fragmentTransaction.SetCustomAnimations(attribute.EnterAnimation, attribute.ExitAnimation);
            }

            if (attribute.TransitionStyle != int.MinValue)
                fragmentTransaction.SetTransitionStyle(attribute.TransitionStyle);
        }

        protected virtual void OnFragmentChanged(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute, MvxViewModelRequest request)
        {
        }

        protected virtual void OnFragmentChanging(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute, MvxViewModelRequest request)
        {
        }

        protected virtual void OnFragmentPopped(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute)
        {
        }

        protected virtual ValueTask<bool> ShowDialogFragment(
            Type view,
            MvxDialogFragmentPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var fragmentName = attribute.ViewType.FragmentJavaName();
            IMvxFragmentView mvxFragmentView = CreateFragment(CurrentActivity.SupportFragmentManager, attribute, attribute.ViewType);
            var dialog = (DialogFragment)mvxFragmentView;

            // MvxNavigationService provides an already instantiated ViewModel here,
            // therefore just assign it
            if (request is MvxViewModelInstanceRequest instanceRequest)
            {
                mvxFragmentView.ViewModel = instanceRequest.ViewModelInstance;
            }
            else
            {
                mvxFragmentView.LoadViewModelFrom(request, null);
            }

            dialog.Cancelable = attribute.Cancelable;

            var ft = CurrentFragmentManager.BeginTransaction();

            OnBeforeFragmentChanging(ft, dialog, attribute, request);

            if (attribute.AddToBackStack)
                ft.AddToBackStack(fragmentName);

            OnFragmentChanging(ft, dialog, attribute, request);

            dialog.Show(ft, fragmentName);

            OnFragmentChanged(ft, dialog, attribute, request);
            return new ValueTask<bool>(true);
        }

        protected virtual ValueTask<bool> ShowViewPagerFragment(
           Type? view,
           MvxViewPagerFragmentPresentationAttribute? attribute,
           MvxViewModelRequest? request)
        {
            ValidateArguments(view, attribute, request);

            // if the attribute doesn't supply any host, assume current activity!
            if (attribute.FragmentHostViewType == null && attribute.ActivityHostViewModelType == null)
                attribute.ActivityHostViewModelType = GetCurrentActivityViewModelType();

            ViewPager? viewPager = null;
            FragmentManager? fragmentManager = null;

            // check for a ViewPager inside a Fragment
            if (attribute!.FragmentHostViewType != null)
            {
                var fragment = GetFragmentByViewType(attribute!.FragmentHostViewType);
                if (fragment == null)
                    throw new MvxException("Fragment not found", attribute!.FragmentHostViewType.Name);

                if (fragment.View == null)
                    throw new MvxException("Fragment.View is null. Please consider calling Navigate later in your code",
                        attribute!.FragmentHostViewType.Name);

                viewPager = fragment.View.FindViewById<ViewPager>(attribute!.ViewPagerResourceId);
                fragmentManager = fragment.ChildFragmentManager;
            }

            // check for a ViewPager inside an Activity
            if (attribute!.ActivityHostViewModelType != null)
            {
                var currentActivityViewModelType = GetCurrentActivityViewModelType();

                // if the host Activity is not the top-most Activity, then show it before proceeding, and return false for now
                if (attribute!.ActivityHostViewModelType != currentActivityViewModelType)
                {
                    PendingRequest = request;
                    ShowHostActivity(attribute);
                    return new ValueTask<bool>(false);
                }

                if (CurrentActivity.IsActivityAlive())
                    viewPager = CurrentActivity!.FindViewById<ViewPager>(attribute!.ViewPagerResourceId);
                fragmentManager = CurrentFragmentManager;
            }

            // no more cases to check. Just throw if ViewPager wasn't found
            if (viewPager == null)
                throw new MvxException("ViewPager not found");

            var tag = attribute!.Tag ?? attribute!.ViewType.FragmentJavaName();
            var fragmentInfo = new MvxViewPagerFragmentInfo(attribute!.Title, tag, attribute!.ViewType, request);

            if (viewPager.Adapter is MvxCachingFragmentStatePagerAdapter adapter)
            {
                adapter.FragmentsInfo.Add(fragmentInfo);
                adapter.NotifyDataSetChanged();
            }
            else
            {
                viewPager.Adapter = new MvxCachingFragmentStatePagerAdapter(
                    CurrentActivity,
                    fragmentManager,
                    new List<MvxViewPagerFragmentInfo>
                    {
                        fragmentInfo
                    }
                );
            }

            return new ValueTask<bool>(true);
        }

        protected virtual async ValueTask<bool> ShowTabLayout(
            Type? view,
            MvxTabLayoutPresentationAttribute? attribute,
            MvxViewModelRequest? request)
        {
            ValidateArguments(view, attribute, request);

            var showViewPagerFragment = await ShowViewPagerFragment(view, attribute, request).ConfigureAwait(true);
            if (!showViewPagerFragment)
                return false;

            ViewPager? viewPager = null;
            TabLayout? tabLayout = null;

            // check for a ViewPager inside a Fragment
            if (attribute?.FragmentHostViewType != null)
            {
                var fragment = GetFragmentByViewType(attribute.FragmentHostViewType);

                viewPager = fragment?.View.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                tabLayout = fragment?.View.FindViewById<TabLayout>(attribute.TabLayoutResourceId);
            }

            // check for a ViewPager inside an Activity
            if (CurrentActivity.IsActivityAlive() && attribute?.ActivityHostViewModelType != null)
            {
                viewPager = CurrentActivity?.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                tabLayout = CurrentActivity?.FindViewById<TabLayout>(attribute.TabLayoutResourceId);
            }

            if (viewPager == null || tabLayout == null)
                throw new MvxException("ViewPager or TabLayout not found");

            tabLayout.SetupWithViewPager(viewPager);
            return true;
        }
        #endregion

        #region Close implementations
        protected virtual ValueTask<bool> CloseActivity(IMvxViewModel viewModel, MvxActivityPresentationAttribute attribute)
        {
            var currentView = CurrentActivity as IMvxView;

            if (currentView == null)
            {
                //MvxLog.Instance?.Warn("Ignoring close for viewmodel - rootframe has no current page");
                return new ValueTask<bool>(false);
            }

            if (currentView.ViewModel != viewModel)
            {
                //MvxLog.Instance?.Warn("Ignoring close for viewmodel - rootframe's current page is not the view for the requested viewmodel");
                return new ValueTask<bool>(false);
            }

            CurrentActivity.Finish();

            return new ValueTask<bool>(true);
        }

        protected virtual ValueTask<bool> CloseFragmentDialog(IMvxViewModel viewModel, MvxDialogFragmentPresentationAttribute attribute)
        {
            string tag = attribute.ViewType.FragmentJavaName();
            var toClose = CurrentFragmentManager.FindFragmentByTag(tag);
            if (toClose != null && toClose is DialogFragment dialog)
            {
                dialog.DismissAllowingStateLoss();
                return new ValueTask<bool>(true);
            }
            return new ValueTask<bool>(false);
        }

        protected virtual bool CloseFragments()
        {
            //try
            //{
                CurrentFragmentManager.PopBackStackImmediate();
            //}
            //catch (System.Exception ex)
            //{
            //    MvxLog.Instance?.Trace("Cannot close any fragments", ex);
            //}
            return true;
        }

        protected virtual ValueTask<bool> CloseFragment(IMvxViewModel viewModel, MvxFragmentPresentationAttribute attribute)
        {
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));

            // try to close nested fragment first
            if (attribute.FragmentHostViewType != null)
            {
                var fragmentHost = GetFragmentByViewType(attribute.FragmentHostViewType);
                if (fragmentHost != null
                    && TryPerformCloseFragmentTransaction(fragmentHost.ChildFragmentManager, attribute))
                    return new ValueTask<bool>(true);
            }

            // Close fragment. If it isn't successful, then close the current Activity
            if (TryPerformCloseFragmentTransaction(CurrentFragmentManager, attribute))
            {
                return new ValueTask<bool>(true);
            }
            else
            {
                CurrentActivity.Finish();
                return new ValueTask<bool>(true);
            }
        }

        protected virtual bool TryPerformCloseFragmentTransaction(
            FragmentManager fragmentManager,
            MvxFragmentPresentationAttribute fragmentAttribute)
        {
            try
            {
                var fragmentName = fragmentAttribute.ViewType.FragmentJavaName();

                if (fragmentManager.BackStackEntryCount > 0)
                {
                    var popBackStackFragmentName = fragmentAttribute.PopBackStackImmediateName?.Trim() == ""
                        ? fragmentName
                        : fragmentAttribute.PopBackStackImmediateName;

                    fragmentManager.PopBackStackImmediate(popBackStackFragmentName, (int)fragmentAttribute.PopBackStackImmediateFlag.ToNativePopBackStackFlags());

                    OnFragmentPopped(null, null, fragmentAttribute);
                    return true;
                }
                else if (CurrentFragmentManager.FindFragmentByTag(fragmentName) != null)
                {
                    var ft = fragmentManager.BeginTransaction();
                    var fragment = fragmentManager.FindFragmentByTag(fragmentName);

                    if (!fragmentAttribute.EnterAnimation.Equals(int.MinValue) && !fragmentAttribute.ExitAnimation.Equals(int.MinValue))
                    {
                        if (!fragmentAttribute.PopEnterAnimation.Equals(int.MinValue) && !fragmentAttribute.PopExitAnimation.Equals(int.MinValue))
                            ft.SetCustomAnimations(fragmentAttribute.EnterAnimation, fragmentAttribute.ExitAnimation, fragmentAttribute.PopEnterAnimation, fragmentAttribute.PopExitAnimation);
                        else
                            ft.SetCustomAnimations(fragmentAttribute.EnterAnimation, fragmentAttribute.ExitAnimation);
                    }
                    if (fragmentAttribute.TransitionStyle != int.MinValue)
                        ft.SetTransitionStyle(fragmentAttribute.TransitionStyle);

                    ft.Remove(fragment);
                    ft.CommitAllowingStateLoss();

                    OnFragmentPopped(ft, fragment, fragmentAttribute);
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                //MvxLog.Instance?.Error("Cannot close fragment transaction", ex);
                return false;
            }

            return false;
        }

        protected virtual ValueTask<bool> CloseViewPagerFragment(
            IMvxViewModel? viewModel,
            MvxViewPagerFragmentPresentationAttribute? attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            ViewPager? viewPager = null;
            FragmentManager? fragmentManager;

            if (attribute.FragmentHostViewType != null)
            {
                var fragment = GetFragmentByViewType(attribute.FragmentHostViewType);
                if (fragment == null)
                    throw new MvxException("Fragment not found", attribute.FragmentHostViewType.Name);

                viewPager = fragment.View.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
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
                    ft.Remove(fragment);
                    ft.CommitAllowingStateLoss();
                    adapter.NotifyDataSetChanged();

                    OnFragmentPopped(ft, fragment, attribute);
                    return new ValueTask<bool>(true);
                }
            }

            return new ValueTask<bool>(false);
        }

        protected virtual MvxViewPagerFragmentInfo? FindFragmentInfoFromAttribute(
            MvxFragmentPresentationAttribute? attribute,
            MvxCachingFragmentStatePagerAdapter? adapter)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            if (adapter == null)
                throw new ArgumentNullException(nameof(adapter));

            MvxViewPagerFragmentInfo? fragmentInfo = null;
            if (attribute.Tag != null)
            {
                fragmentInfo = adapter.FragmentsInfo?.FirstOrDefault(f => f.Tag == attribute.Tag);
            }

            if (fragmentInfo != null)
                return fragmentInfo;

            bool IsMatch(MvxViewPagerFragmentInfo? info)
            {
                if (attribute.ViewType == null) return false;

                var viewTypeMatches = info?.FragmentType == attribute.ViewType;

                if (attribute.ViewModelType != null)
                    return viewTypeMatches && info?.Request?.ViewModelType == attribute.ViewModelType;

                return viewTypeMatches;
            }

            fragmentInfo = adapter.FragmentsInfo?.FirstOrDefault(IsMatch);
            return fragmentInfo;
        }
        #endregion

        protected virtual IMvxFragmentView CreateFragment(FragmentManager fragmentManager, MvxBasePresentationAttribute attribute,
            Type fragmentType)
        {
            try
            {
                var fragmentClass = Class.FromType(fragmentType);
                var fragment = (IMvxFragmentView)fragmentManager.FragmentFactory.Instantiate(fragmentClass.ClassLoader, fragmentClass.Name);
                return fragment;
            }
            catch (System.Exception ex)
            {
                throw new MvxException($"Cannot create Fragment '{fragmentType.Name}'", ex);
            }
        }

        protected virtual Fragment GetFragmentByViewType(Type type)
        {
            var fragmentName = type.FragmentJavaName();
            var fragment = CurrentFragmentManager?.FindFragmentByTag(fragmentName);

            if (fragment != null)
            {
                return fragment;
            }

            return FindFragmentInChildren(fragmentName, CurrentFragmentManager);
        }

        protected virtual Fragment? FindFragmentInChildren(string fragmentName, FragmentManager fragManager)
        {
            if (fragManager == null)
                return null;

            if (fragManager.BackStackEntryCount == 0)
                return null;

            for (int i = 0; i < fragManager.BackStackEntryCount; i++)
            {
                var parentFrag = fragManager.FindFragmentById(fragManager.GetBackStackEntryAt(i).Id);

                //let's try again finding it
                var frag = parentFrag?.ChildFragmentManager?.FindFragmentByTag(fragmentName);

                if (frag == null)
                {
                    //reloop for other fragments
                    frag = FindFragmentInChildren(fragmentName, parentFrag?.ChildFragmentManager);
                }

                //if we found the frag lets return it!
                if (frag != null)
                {
                    return frag;
                }
            }

            return null;
        }

        private static void ValidateArguments(Type? view, MvxBasePresentationAttribute? attribute, MvxViewModelRequest? request)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));

            ValidateArguments(attribute, request);
        }

        private static void ValidateArguments(MvxBasePresentationAttribute? attribute, MvxViewModelRequest? request)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            if (request == null)
                throw new ArgumentNullException(nameof(request));
        }
    }
}
