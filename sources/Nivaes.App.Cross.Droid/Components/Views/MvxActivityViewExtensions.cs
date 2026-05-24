using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Nivaes.IoC;

namespace Nivaes.App.Cross.Droid;

public static class MvxActivityViewExtensions
{
    [RequiresUnreferencedCode("MvxBindings require unreferenced code")]
    public static void AddEventListeners(this IMvxEventSourceActivity activity)
    {
        if (activity is IMvxAndroidView)
        {
            var adapter = new MvxActivityAdapter(activity);
        }
        if (activity is ICrossBindingContextOwner)
        {
            var bindingAdapter = new MvxBindingActivityAdapter(activity);
        }
        if (activity is IMvxChildViewModelOwner)
        {
            var childOwnerAdapter = new MvxChildViewModelOwnerAdapter(activity);
        }
    }

    [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
    public static void OnViewCreate(this IMvxAndroidView androidView, Bundle bundle)
    {
        androidView.OnLifetimeEvent((listener, activity) => listener.OnCreate(activity, bundle));

        ICrossViewModel? cached = null;
        if (Mvx.IoCProvider?.TryResolve<IMvxSingleViewModelCache>(out var cache) == true)
            cached = cache?.GetAndClear(bundle);

        var view = (ICrossView)androidView;
        var savedState = GetSavedStateFromBundle(bundle);
        view.OnViewCreate(() => cached ?? androidView.LoadViewModel(savedState));
    }

    private static ICrossBundle? GetSavedStateFromBundle(Bundle? bundle)
    {
        if (bundle == null)
            return null;

        if (Mvx.IoCProvider?.TryResolve<IMvxSavedStateConverter>(out var converter) != true || converter == null)
        {
            CrossLogHost.Default?.Log(LogLevel.Trace, "No saved state converter available - this is OK if seen during start");
            return null;
        }
        var savedState = converter.Read(bundle);
        return savedState;
    }

    public static void OnViewNewIntent(this IMvxAndroidView androidView)
    {
        CrossLogHost.Default?.Log(LogLevel.Trace, "OnViewNewIntent called - MvvmCross lifecycle won't run automatically in this case");
    }

    public static void OnViewDestroy(this IMvxAndroidView androidView)
    {
        androidView.OnLifetimeEvent((listener, activity) => listener.OnDestroy(activity));
        var view = androidView as ICrossView;
        view.OnViewDestroy();

        if (Mvx.IoCProvider?.TryResolve<ICrossAppStart>(out var appStart) != true ||
            Mvx.IoCProvider?.TryResolve<IMvxAndroidCurrentTopActivity>(out var topActivity) != true ||
            appStart == null || topActivity == null)
        {
            return;
        }

        var currentActivity = topActivity.Activity;
        if (IsActivityTearingDown(currentActivity))
        {
            appStart.ResetStart();
        }
        else if (currentActivity == null && IsActivityTearingDown(view as Activity))
        {
            appStart.ResetStart();
        }
    }

    private static bool IsActivityTearingDown(Activity? activity)
    {
        if (activity == null) return false;
        if (activity.IsDestroyed) return true;
        if (activity.IsFinishing) return true;
        return false;
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

    private static void OnLifetimeEvent(
        this IMvxAndroidView androidView,
        Action<IMvxAndroidActivityLifetimeListener, Activity> report)
    {
        if (Mvx.IoCProvider?.TryResolve(out IMvxAndroidActivityLifetimeListener? activityLifetimeListener) == true &&
            activityLifetimeListener != null)
        {
            report(activityLifetimeListener, androidView.ToActivity());
        }
    }

    public static Activity ToActivity(this IMvxAndroidView androidView)
    {
        var activity = androidView as Activity;
        if (activity == null)
            throw new CrossException("OnViewCreate called from an IMvxView which is not an Android Activity");
        return activity;
    }

    [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "Activity types are preserved by the Android presenter infrastructure.")]
    [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
    private static ICrossViewModel? LoadViewModel(this IMvxAndroidView androidView, ICrossBundle? savedState)
    {
        var activity = androidView.ToActivity();

        var viewModelType = androidView.FindAssociatedViewModelTypeOrNull();
        if (viewModelType == typeof(CrossNullViewModel))
            return new CrossNullViewModel();

        if (viewModelType == null
            || viewModelType == typeof(ICrossViewModel))
        {
            CrossLogHost.Default?.Log(LogLevel.Trace, "No ViewModel class specified for {ViewType} in LoadViewModel",
                androidView.GetType().Name);
        }

        if (Mvx.IoCProvider?.TryResolve(out IMvxAndroidViewModelLoader? viewModelLoader) == true &&
            viewModelLoader != null)
        {
            return viewModelLoader.Load(activity.Intent, savedState, viewModelType);
        }

        return null;
    }
}