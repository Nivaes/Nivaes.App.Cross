using Microsoft.Extensions.Logging;
using Activity = AndroidX.AppCompat.App.AppCompatActivity;
using DialogFragment = AndroidX.Fragment.App.DialogFragment;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;

namespace Nivaes.App.Cross.Droid;

public class AndroidViewPresenterManager 
    : CrossViewPresenterManager, IAndroidViewPresenterManager
{
    public const string ViewModelRequestBundleKey = "__viewModelRequest";
    public const string SharedElementsBundleKey = "__sharedElementsKey";

    private readonly IMvxAndroidCurrentTopActivity _androidCurrentTopActivity;
    private readonly IMvxAndroidActivityLifetimeListener _activityLifetimeListener;

    private readonly IMvxAndroidViewModelRequestTranslator _viewModelRequestTranslator;

    protected ViewModelRequest? PendingRequest { get; set; }

    protected virtual Activity? CurrentActivity => _androidCurrentTopActivity.Activity as Activity;

    protected IMvxAndroidActivityLifetimeListener? ActivityLifetimeListener => _activityLifetimeListener;

    protected virtual FragmentManager? CurrentFragmentManager
    {
        get
        {
            if (CurrentActivity?.IsActivityDead() ?? false)
                return null;

            return CurrentActivity!.SupportFragmentManager;
        }
    }

    public AndroidViewPresenterManager(
        IMvxAndroidCurrentTopActivity androidCurrentTopActivity, 
        IMvxAndroidActivityLifetimeListener activityLifetimeListener, 
        IMvxAndroidViewModelRequestTranslator viewModelRequestTranslator,
        ILogger<AndroidViewPresenterManager> logger)
        : base(logger)
    {
        _androidCurrentTopActivity = androidCurrentTopActivity;
        _activityLifetimeListener = activityLifetimeListener;
        _viewModelRequestTranslator = viewModelRequestTranslator;

        ActivityLifetimeListener?.ActivityChanged += ActivityLifetimeListenerOnActivityChanged;
    }

    protected virtual void ActivityLifetimeListenerOnActivityChanged(object? sender, MvxActivityEventArgs e)
    {
        if (e.ActivityState == MvxActivityState.OnResume && PendingRequest != null)
        {
            Show(PendingRequest);
            PendingRequest = null;
        }
        else if (e is { ActivityState: MvxActivityState.OnCreate, Extras: Bundle })
        {
            //TODO: Restore fragments from bundle
        }
        else if (e is { ActivityState: MvxActivityState.OnSaveInstanceState, Extras: Bundle })
        {
            //TODO: Save fragments into bundle
        }
        else if (e.ActivityState == MvxActivityState.OnDestroy)
        {
            //TODO: Should be check for Fragments on this Activity and destroy them?
        }
    }

    public override BasePresentationAttribute CreatePresentationAttribute(IViewModelRequest request)
    {
        if (request.ViewType.IsSubclassOf(typeof(DialogFragment)))
        {
            Logger.LogWarning($"PresentationAttribute not found for {request.ViewType.FullName}. Assuming DialogFragment presentation");
            return new DialogFragmentPresentationAttribute(enterAnimation: int.MinValue);
        }

        if (request.ViewType.IsSubclassOf(typeof(Fragment)))
        {
            Logger.Log(LogLevel.Trace, $"PresentationAttribute not found for {request.ViewType.FullName}. Assuming Fragment presentation");
            return new FragmentPresentationAttribute(GetCurrentActivityViewModelType(), global::Android.Resource.Id.Content);
        }

        if (request.ViewType.IsSubclassOf(typeof(Activity)))
        {
            Logger.Log(LogLevel.Trace, $"PresentationAttribute not found for {request.ViewType.FullName}. Assuming Activity presentation");
            return new ActivityPresentationAttribute();
        }

        throw new AppException($"Don't know how to create a presentation attribute for type {request.ViewType.FullName}");
    }

    protected Type? GetCurrentActivityViewModelType()
    {
        Type? currentActivityType = null;
        if (CurrentActivity!.IsActivityAlive())
            currentActivityType = CurrentActivity!.GetType();

        if (currentActivityType == null)
            return null;

        Singleton<ViewViewModelsKeyContainerManager>.Instance.TryGetValue(currentActivityType, out var viewModelType);
        return viewModelType;
    }
}
