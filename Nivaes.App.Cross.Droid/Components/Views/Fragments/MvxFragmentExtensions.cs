﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Views;

namespace MvvmCross.Platforms.Android.Views.Fragments
{
    using System;
    using System.Threading.Tasks;
    using MvvmCross.Exceptions;
    using MvvmCross.Platforms.Android.Binding.BindingContext;
    using MvvmCross.Views;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.ViewModels;
    using Fragment = AndroidX.Fragment.App.Fragment;

    public static class MvxFragmentExtensions
    {
        public static void AddEventListeners(this IMvxEventSourceFragment fragment)
        {
            if (fragment is IMvxFragmentView)
            {
                var adapter = new MvxBindingFragmentAdapter(fragment);
            }
        }

        public static async Task OnCreate(this IMvxFragmentView fragmentView, IMvxBundle? bundle, MvxViewModelRequest? request = null)
        {
            if (fragmentView == null) throw new ArgumentNullException(nameof(fragmentView));

            var cache = Mvx.IoCProvider.Resolve<IMvxMultipleViewModelCache>();

            if (fragmentView.ViewModel != null)
            {
                // check if ViewModel instance was cached. If so, clear it and ignore previous instance
                cache.GetAndClear(fragmentView.ViewModel.GetType(), fragmentView.UniqueImmutableCacheTag);
                return;
            }

            var fragment = fragmentView.ToFragment();
            if (fragment == null)
                throw new MvxException($"{nameof(OnCreate)} called on an {nameof(IMvxFragmentView)} which is not an Android Fragment: {fragmentView}");

            // as it is called during onCreate it is safe to assume that fragment has Activity attached.
            var viewModelType = fragmentView.FindAssociatedViewModelType(fragment.Activity.GetType());
            var view = fragmentView as IMvxView;

            var cached = cache.GetAndClear(viewModelType, fragmentView.UniqueImmutableCacheTag);

            await view.OnViewCreate(async () =>  cached ?? await fragmentView.LoadViewModel(bundle, fragment.Activity.GetType(), request).ConfigureAwait(false)).ConfigureAwait(false);
        }

        public static Fragment ToFragment(this IMvxFragmentView fragmentView)
        {
            return (Fragment)fragmentView;
        }

        public static void EnsureBindingContextIsSet(this IMvxFragmentView fragment, LayoutInflater inflater)
        {
            var actualFragment = fragment.ToFragment();
            if (actualFragment == null)
                throw new MvxException($"{nameof(EnsureBindingContextIsSet)} called on an {nameof(IMvxFragmentView)} which is not an Android Fragment: {fragment}");

            if (fragment.BindingContext == null)
            {
                fragment.BindingContext = new MvxAndroidBindingContext(actualFragment.Activity,
                    new MvxSimpleLayoutInflaterHolder(inflater),
                    fragment.DataContext);
            }
            else
            {
                if (fragment.BindingContext is IMvxAndroidBindingContext androidContext)
                    androidContext.LayoutInflaterHolder = new MvxSimpleLayoutInflaterHolder(inflater);
            }
        }

        public static void EnsureBindingContextIsSet(this IMvxFragmentView fragment)
        {
            var actualFragment = fragment.ToFragment();
            if (actualFragment == null)
                throw new MvxException($"{nameof(EnsureBindingContextIsSet)} called on an {nameof(IMvxFragmentView)} which is not an Android Fragment: {fragment}");

            if (fragment.BindingContext == null)
            {
                fragment.BindingContext = new MvxAndroidBindingContext(actualFragment.Activity,
                    new MvxSimpleLayoutInflaterHolder(
                        actualFragment.Activity.LayoutInflater),
                    fragment.DataContext);
            }
            else
            {
                if (fragment.BindingContext is IMvxAndroidBindingContext androidContext)
                    androidContext.LayoutInflaterHolder = new MvxSimpleLayoutInflaterHolder(actualFragment.Activity.LayoutInflater);
            }
        }

        //public static void EnsureSetupInitialized(this IMvxFragmentView fragmentView)
        //{
        //    var fragment = fragmentView.ToFragment();
        //    if (fragment == null)
        //        throw new MvxException($"{nameof(EnsureSetupInitialized)} called on an {nameof(IMvxFragmentView)} which is not an Android Fragment: {fragmentView}");

        //    //var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(fragment.Activity.ApplicationContext);
        //    //_ = setup.EnsureInitialized();
        //}

        public static TFragment FindFragmentById<TFragment>(this MvxActivity activity, int resourceId)
            where TFragment : Fragment
        {
            if (activity == null) throw new ArgumentNullException(nameof(activity));

            var fragment = activity.SupportFragmentManager.FindFragmentById(resourceId);
            if (fragment == null)
            {
                //MvxLog.Instance?.Warn("Failed to find fragment id {0} in {1}", resourceId, activity.GetType().Name);
                return default;
            }

            return SafeCast<TFragment>(fragment);
        }

        public static TFragment FindFragmentByTag<TFragment>(this MvxActivity activity, string tag)
            where TFragment : Fragment
        {
            if (activity == null) throw new ArgumentNullException(nameof(activity));

            var fragment = activity.SupportFragmentManager.FindFragmentByTag(tag);
            if (fragment == null)
            {
                //MvxLog.Instance?.Warn("Failed to find fragment tag {0} in {1}", tag, activity.GetType().Name);
                return default;
            }

            return SafeCast<TFragment>(fragment);
        }

        private static TFragment SafeCast<TFragment>(Fragment fragment) where TFragment : Fragment
        {
            if (!(fragment is TFragment))
            {
                //MvxLog.Instance?.Warn("Fragment type mismatch got {0} but expected {1}", fragment.GetType().FullName,
                //            typeof(TFragment).FullName);
                return default;
            }

            return (TFragment)fragment;
        }

        public static async ValueTask LoadViewModelFrom(this Android.Views.IMvxFragmentView view, MvxViewModelRequest request, IMvxBundle? savedState = null)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));

            var loader = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();
            var viewModel = await loader.LoadViewModel(request, savedState).ConfigureAwait(false);
            if (viewModel == null)
            {
                //MvxLog.Instance?.Warn("ViewModel not loaded for {0}", request.ViewModelType.FullName);
                return;
            }

            view.ViewModel = viewModel;
        }
    }
}
