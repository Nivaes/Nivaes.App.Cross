// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.OS;

namespace MvvmCross.Platforms.Android.Views.Fragments
{
    using System;
    using AndroidX.Fragment.App;
    using MvvmCross.Base;
    using MvvmCross.Logging;
    using MvvmCross.Platforms.Android.Core;
    using MvvmCross.Platforms.Android.Presenters;
    using MvvmCross.Platforms.Android.Views.Fragments.EventSource;
    using MvvmCross.ViewModels;
    using MvvmCross.Views;

    public class MvxBindingFragmentAdapter
        : MvxBaseFragmentAdapter
    {
        public IMvxFragmentView FragmentView => (IMvxFragmentView)Fragment;

        public MvxBindingFragmentAdapter(IMvxEventSourceFragment eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxFragmentView))
                throw new ArgumentException("eventSource must be an IMvxFragmentView", nameof(eventSource));
        }

        protected override void HandleCreateCalled(object sender, MvxValueEventArgs<Bundle> bundleArgs)
        {
            //FragmentView.EnsureSetupInitialized();

            // Create is called after Fragment is attached to Activity
            // it's safe to assume that Fragment has activity

            if (!(Fragment.Activity is IMvxAndroidView hostMvxView))
            {
                MvxLog.Instance.Warn($"Fragment host for fragment type {Fragment.GetType()} is not of type IMvxAndroidView");
                return;
            }

            // if restoring state, Activity.ViewModel might be null, so a harder mechanism is necessary
            var viewModelType = hostMvxView.ViewModel != null
                                       ? hostMvxView.ViewModel.GetType()
                                       : hostMvxView.FindAssociatedViewModelTypeOrNull();

            if (viewModelType == null)
            {
                MvxLog.Instance.Warn($"ViewModel type for Activity {Fragment.Activity.GetType()} not found when trying to show fragment: {Fragment.GetType()}");
                return;
            }

            Bundle? bundle = null;
            MvxViewModelRequest? request = null;
            if (bundleArgs?.Value != null)
            {
                // saved state
                bundle = bundleArgs.Value;
            }
            else
            {
                var fragment = FragmentView as Fragment;
                if (fragment?.Arguments != null)
                {
                    bundle = fragment.Arguments;
                    var json = bundle.GetString(MvxAndroidViewPresenter.ViewModelRequestBundleKey);
                    if (!string.IsNullOrEmpty(json))
                    {
                        if (!Mvx.IoCProvider.TryResolve(out IMvxNavigationSerializer serializer))
                        {
                            MvxLog.Instance.Warn(
                                "Navigation Serializer not available, deserializing ViewModel Request will be hard");
                        }
                        else
                        {
                            request = serializer.Serializer.DeserializeObject<MvxViewModelRequest>(json!);
                        }
                    }
                }
            }

            if (!Mvx.IoCProvider.TryResolve(out IMvxSavedStateConverter converter))
            {
                MvxLog.Instance.Warn("Saved state converter not available - saving state will be hard");
            }
            else
            {
                if (bundle != null)
                {
                    var mvxBundle = converter.Read(bundle);
                    FragmentView.OnCreate(mvxBundle, request);
                }
            }
        }

        protected override void HandleCreateViewCalled(object sender,
                                               MvxValueEventArgs<MvxCreateViewParameters> args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));

            FragmentView.EnsureBindingContextIsSet(args.Value.Inflater);
        }

        protected override void HandleSaveInstanceStateCalled(object sender, MvxValueEventArgs<Bundle> bundleArgs)
        {
            if (bundleArgs == null) throw new ArgumentNullException(nameof(bundleArgs));

            // it is guarannted that SaveInstanceState call will be executed before OnStop (thus before Fragment detach)
            // it is safe to assume that Fragment has activity attached

            var mvxBundle = FragmentView.CreateSaveStateBundle();
            if (mvxBundle != null)
            {
                if (!Mvx.IoCProvider.TryResolve(out IMvxSavedStateConverter converter))
                {
                    MvxLog.Instance.Warn("Saved state converter not available - saving state will be hard");
                }
                else
                {
                    converter.Write(bundleArgs.Value, mvxBundle);
                }
            }
            var cache = Mvx.IoCProvider.Resolve<IMvxMultipleViewModelCache>();
            cache.Cache(FragmentView.ViewModel, FragmentView.UniqueImmutableCacheTag);
        }

        protected override void HandleDestroyViewCalled(object sender, EventArgs e)
        {
            FragmentView.BindingContext?.ClearAllBindings();
            base.HandleDestroyViewCalled(sender, e);
        }

        protected override void HandleDisposeCalled(object sender, EventArgs e)
        {
            FragmentView.BindingContext?.ClearAllBindings();
            base.HandleDisposeCalled(sender, e);
        }
    }
}
