// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.App;
using Android.OS;

namespace MvvmCross.Platforms.Android.Views
{
    using System;
    using System.Threading.Tasks;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Core;
    using MvvmCross.Exceptions;
    using MvvmCross.Platforms.Android.Core;
    using MvvmCross.Platforms.Android.Views.Base;
    using MvvmCross.Views;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.ViewModels;

    public static class MvxActivityViewExtensions
    {
        public static void AddEventListeners(this IMvxEventSourceActivity activity)
        {
            if (activity is IMvxAndroidView)
            {
                var adapter = new MvxActivityAdapter(activity);
            }
            if (activity is IMvxBindingContextOwner)
            {
                var bindingAdapter = new MvxBindingActivityAdapter(activity);
            }
            if (activity is IMvxChildViewModelOwner)
            {
                var childOwnerAdapter = new MvxChildViewModelOwnerAdapter(activity);
            }
        }

        public static async Task OnViewCreate(this IMvxAndroidView androidView, Bundle bundle)
        {
            if (androidView == null) throw new NullReferenceException(nameof(androidView));

            androidView.EnsureSetupInitialized();
            androidView.OnLifetimeEvent((listener, activity) => listener.OnCreate(activity, bundle));

            var cache = Mvx.IoCProvider.Resolve<IMvxSingleViewModelCache>();
            var cached = cache.GetAndClear(bundle);

            var view = (IMvxView)androidView;
            var savedState = GetSavedStateFromBundle(bundle);
            await view.OnViewCreate(async () => cached ?? await androidView.LoadViewModel(savedState).ConfigureAwait(false)).ConfigureAwait(false);
        }

        private static IMvxBundle? GetSavedStateFromBundle(Bundle bundle)
        {
            if (bundle == null)
                return null;

            if (!Mvx.IoCProvider.TryResolve<IMvxSavedStateConverter>(out IMvxSavedStateConverter converter))
            {
                //MvxLog.Instance?.Trace("No saved state converter available - this is OK if seen during start");
                return null;
            }
            var savedState = converter.Read(bundle);
            return savedState;
        }

        public static void OnViewNewIntent(this IMvxAndroidView androidView)
        {
            //MvxLog.Instance?.Trace("OnViewNewIntent called - MvvmCross lifecycle won't run automatically in this case.");
        }

        public static void OnViewDestroy(this IMvxAndroidView androidView)
        {
            androidView.OnLifetimeEvent((listener, activity) => listener.OnDestroy(activity));
            var view = androidView as IMvxView;
            view.OnViewDestroy();

            var currentActivity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>()?.Activity;
            if (currentActivity == null && view is Activity destroyedActivity && destroyedActivity.IsFinishing && Mvx.IoCProvider.TryResolve<IMvxAppStart>(out var appStart))
            {
                appStart?.ResetStart();
            }
        }

        public static void OnViewStart(this IMvxAndroidView androidView)
        {
            androidView.OnLifetimeEvent((listener, activity) => listener.OnStart(activity));
        }

        public static void OnViewRestart(this IMvxAndroidView androidView)
        {
            androidView.OnLifetimeEvent((listener, activity) => listener.OnRestart(activity));
        }

        public static void OnViewStop(this IMvxAndroidView androidView)
        {
            androidView.OnLifetimeEvent((listener, activity) => listener.OnStop(activity));
        }

        public static void OnViewResume(this IMvxAndroidView androidView)
        {
            androidView.OnLifetimeEvent((listener, activity) => listener.OnResume(activity));
        }

        public static void OnViewPause(this IMvxAndroidView androidView)
        {
            androidView.OnLifetimeEvent((listener, activity) => listener.OnPause(activity));
        }

        private static void OnLifetimeEvent(this IMvxAndroidView androidView,
                                            Action<IMvxAndroidActivityLifetimeListener, Activity> report)
        {
            var activityTracker = Mvx.IoCProvider.Resolve<IMvxAndroidActivityLifetimeListener>();
            report(activityTracker, androidView.ToActivity());
        }

        public static Activity ToActivity(this IMvxAndroidView androidView)
        {
            var activity = androidView as Activity;
            if (activity == null)
                throw new MvxException("OnViewCreate called from an IMvxView which is not an Android Activity");
            return activity;
        }

        private static async ValueTask<IMvxViewModel?> LoadViewModel(this IMvxAndroidView androidView, IMvxBundle? savedState)
        {
            var activity = androidView.ToActivity();

            var viewModelType = androidView.FindAssociatedViewModelTypeOrNull();
            if (viewModelType == typeof(MvxNullViewModel))
                return new MvxNullViewModel();

            //if (viewModelType == null
            //    || viewModelType == typeof(IMvxViewModel))
            //{
            //    MvxLog.Instance?.Trace("No ViewModel class specified for {0} in LoadViewModel",
            //                   androidView.GetType().Name);
            //}

            var translatorService = Mvx.IoCProvider.Resolve<IMvxAndroidViewModelLoader>();
            var viewModel = await translatorService.Load(activity!.Intent, savedState, viewModelType!).ConfigureAwait(false);

            return viewModel;
        }

        private static void EnsureSetupInitialized(this IMvxAndroidView androidView)
        {
            if (androidView is IMvxSetupMonitor)
            {
                // setup monitor views manage their own setup initialization
                return;
            }

            var activity = androidView.ToActivity();
            var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(activity!.ApplicationContext!);
            //_ = setup.EnsureInitialized();
        }
    }
}
