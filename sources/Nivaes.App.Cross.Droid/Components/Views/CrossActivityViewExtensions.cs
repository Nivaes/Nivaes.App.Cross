using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;

namespace Nivaes.App.Cross.Droid;

public static class CrossActivityViewExtensions
{
    [RequiresUnreferencedCode("Bindings require unreferenced code")]
    public static void AddEventListeners(this ICrossEventSourceActivity activity)
    {
        // ToDo: Mirar si es mejor meter esto en cada clase, para que no sea tan generico.
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
    public static void OnViewCreate(this IMvxAndroidView androidView, Bundle? bundle)
    {
        androidView.OnLifetimeEvent((listener, activity) => listener.OnCreate(activity, bundle));

        ICrossViewModel? cached = null;

        var cache = IPlatformApplication.Current!.Services.GetRequiredService<IMvxSingleViewModelCache>();
        //if (Mvx.IoCProvider?.TryResolve<IMvxSingleViewModelCache>(out var cache) == true)
        cached = cache?.GetAndClear(bundle);

        var view = (ICrossView)androidView;
        var savedState = GetSavedStateFromBundle(bundle);
        view.OnViewCreate(() => cached ?? androidView.LoadViewModel(savedState));
    }

    private static ICrossBundle? GetSavedStateFromBundle(Bundle? bundle)
    {
        if (bundle == null)
            return null;

        var converter = IPlatformApplication.Current!.Services.GetRequiredService<IMvxSavedStateConverter>();

        var savedState = converter.Read(bundle);
        return savedState;
    }

    public static void OnViewNewIntent(this IMvxAndroidView androidView)
    {
        CrossLoggerHost.GetLogger(nameof(CrossActivityViewExtensions)).LogTrace("OnViewNewIntent called - Cross lifecycle won't run automatically in this case");
    }

    public static void OnViewDestroy(this IMvxAndroidView androidView)
    {
        androidView.OnLifetimeEvent((listener, activity) => listener.OnDestroy(activity));
        var view = androidView as ICrossView;
        view.OnViewDestroy();

        //var appStart = IPlatformApplication.Current!.Services.GetRequiredService<ICrossAppStart>();
        var application = IPlatformApplication.Current!.Services.GetRequiredService<IApplication>();
        var topActivity = IPlatformApplication.Current!.Services.GetRequiredService<IMvxAndroidCurrentTopActivity>();


        //if (Mvx.IoCProvider?.TryResolve<ICrossAppStart>(out var appStart) != true ||
        //    Mvx.IoCProvider?.TryResolve<IMvxAndroidCurrentTopActivity>(out var topActivity) != true ||
        //    appStart == null || topActivity == null)
        //{
        //    return;
        //}

        var currentActivity = topActivity.Activity;
        if (IsActivityTearingDown(currentActivity))
        {
            application.Startup();
        }
        else if (currentActivity == null && IsActivityTearingDown(view as Activity))
        {
            application.Startup();
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
        var activityLifetimeListener = IPlatformApplication.Current!.Services.GetRequiredService<IMvxAndroidActivityLifetimeListener>();

        report(activityLifetimeListener, androidView.ToActivity());
    }

    public static Activity ToActivity(this IMvxAndroidView androidView)
    {
        var activity = androidView as Activity;
        if (activity == null)
            throw new CrossException("OnViewCreate called from an IMvxView which is not an Android Activity");
        return activity;
    }

    [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
    private static ICrossViewModel LoadViewModel(this IMvxAndroidView androidView, ICrossBundle? savedState)
    {
        var activity = androidView.ToActivity();

        var viewModelType = androidView.FindAssociatedViewModelTypeOrNull();
        //if (viewModelType == typeof(CrossNullViewModel))
        //    return new CrossNullViewModel();
        if (viewModelType == null)
            throw new CrossException($"Not ViewModel asociate to {androidView.GetType().FullName}");

        //if (viewModelType == null
        //    || viewModelType == typeof(ICrossViewModel))
        //{
        //    CrossLoggerHost.Default.Log(LogLevel.Trace, "No ViewModel class specified for {ViewType} in LoadViewModel",
        //        androidView.GetType().Name);
        //}

        var viewType = androidView.GetType();

        var viewModelLoader = IPlatformApplication.Current!.Services.GetRequiredService<IMvxAndroidViewModelLoader>();
        if (!Singleton<CrossViewsViewModelManager>.Instance.TryGetValue(viewType, out viewModelType))
        {
            //var logger = CrossLogHost.GetLogger($"{nameof(CrossActivityViewExtensions)}.{nameof(LoadViewModel)}");
            //logger.Log(LogLevel.Trace, $"No ViewModel class specified for {viewType} in LoadViewModel",
            //    androidView.GetType().Name);
            throw new CrossException($"No ViewModel class specified for {viewType} in LoadViewModel", androidView.GetType().Name);
        }

        //if (Mvx.IoCProvider?.TryResolve(out IMvxAndroidViewModelLoader? viewModelLoader) == true &&
        //    viewModelLoader != null)
        //{
        return viewModelLoader!.Load(activity.Intent, savedState, viewModelType);
        //}

        //return null;
    }
}