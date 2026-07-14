using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Android.Content;
using Android.OS;
using Android.Util;
using AndroidX.ViewPager.Widget;
using Google.Android.Material.Tabs;
using Java.Lang;
using Microsoft.Extensions.Logging;
using Activity = AndroidX.AppCompat.App.AppCompatActivity;
using DialogFragment = AndroidX.Fragment.App.DialogFragment;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;
using FragmentTransaction = AndroidX.Fragment.App.FragmentTransaction;

namespace Nivaes.App.Cross.Droid;

public class AndroidViewPresenterManager : CrossAttributeViewPresenterManager, IAndroidViewPresenterManager
{
    public const string ViewModelRequestBundleKey = "__mvxViewModelRequest";
    public const string SharedElementsBundleKey = "__sharedElementsKey";

    private readonly IMvxAndroidCurrentTopActivity _androidCurrentTopActivity;
    private readonly IMvxAndroidActivityLifetimeListener _activityLifetimeListener;
    private readonly ICrossNavigationSerializer _navigationSerializer;
    private readonly IMvxAndroidViewModelRequestTranslator _viewModelRequestTranslator;

    protected CrossViewModelRequest? PendingRequest { get; set; }

    protected virtual FragmentManager? CurrentFragmentManager
    {
        get
        {
            if (CurrentActivity.IsActivityDead())
                return null;

            return CurrentActivity!.SupportFragmentManager;
        }
    }

    protected virtual Activity? CurrentActivity => _androidCurrentTopActivity.Activity as Activity;

    protected IMvxAndroidActivityLifetimeListener? ActivityLifetimeListener => _activityLifetimeListener;

    protected ICrossNavigationSerializer? NavigationSerializer => _navigationSerializer;

    public AndroidViewPresenterManager(ICrossViewsContainer crossViewsContainer,
        IMvxAndroidCurrentTopActivity androidCurrentTopActivity, IMvxAndroidActivityLifetimeListener activityLifetimeListener, 
        ICrossNavigationSerializer navigationSerializer,
        IMvxAndroidViewModelRequestTranslator viewModelRequestTranslator,
        ILogger<AndroidViewPresenterManager> logger)
        : base(crossViewsContainer, logger)
    {
        _androidCurrentTopActivity = androidCurrentTopActivity;
        _activityLifetimeListener = activityLifetimeListener;
        _navigationSerializer = navigationSerializer;
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

    [Obsolete("Carga por Roslyn")]
    public override void RegisterAttributeTypes()
    {
        AttributeTypesToActionsDictionary.Register<ActivityPresentationAttribute>(ShowActivity, CloseActivity);
        AttributeTypesToActionsDictionary.Register<FragmentPresentationAttribute>(ShowFragment, CloseFragment);
        AttributeTypesToActionsDictionary.Register<DialogFragmentPresentationAttribute>(ShowDialogFragment, CloseFragmentDialog);
        AttributeTypesToActionsDictionary.Register<TabLayoutPresentationAttribute>(ShowTabLayout, CloseViewPagerFragment);
        AttributeTypesToActionsDictionary.Register<ViewPagerFragmentPresentationAttribute>(ShowViewPagerFragment, CloseViewPagerFragment);
    }

    [Obsolete]
    public override BasePresentationAttribute GetPresentationAttribute(CrossViewModelRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var viewType = base.ViewsContainer?.GetViewType(request.ViewModelType!);
        if (viewType == null)
            throw new InvalidOperationException($"Could not get view type for ViewModel Type: {request.ViewModelType}");

        //var overrideAttribute = GetOverridePresentationAttribute(request, viewType);
        //if (overrideAttribute != null)
        //    return overrideAttribute;

        IList<BasePresentationAttribute> attributes = viewType.GetCustomAttributes<BasePresentationAttribute>(true).ToList();
        if (attributes.Count > 0)
        {
            BasePresentationAttribute? attribute = null;

            if (attributes.Count > 1)
            {
                var fragmentAttributes = attributes.OfType<FragmentPresentationAttribute>().ToArray();

                // check if fragment can be displayed as child fragment first
                attribute = GetAttributeForFragmentChildPresentation(fragmentAttributes);

                // if attribute is still null, check if fragment can be displayed in current activity
                attribute ??= GetAttributeForFragmentPresentation(fragmentAttributes);
            }

            // fallback to first attribute
            attribute ??= attributes[0];
            attribute.ViewType = viewType;

            return attribute;
        }

        return CreatePresentationAttribute(request.ViewModelType, viewType);
    }

    [Obsolete]
    private BasePresentationAttribute? GetAttributeForFragmentPresentation(
        IEnumerable<FragmentPresentationAttribute> fragmentAttributes)
    {
        BasePresentationAttribute? attribute = null;

        var currentActivityHostViewModelType = GetCurrentActivityViewModelType();

        foreach (var item in fragmentAttributes.Where(
            att => att.ActivityHostViewModelType != null))
        {
            if (CurrentActivity!.IsActivityDead())
                break;

            if (CurrentActivity!.FindViewById(item.FragmentContentId) != null &&
                item.ActivityHostViewModelType == currentActivityHostViewModelType)
            {
                attribute = item;
                break;
            }
        }

        return attribute;
    }

    [Obsolete("Migrate to PressenterAction", true)]
    private BasePresentationAttribute? GetAttributeForFragmentChildPresentation(
        IEnumerable<FragmentPresentationAttribute> fragmentAttributes)
    {
        BasePresentationAttribute? attribute = null;

        foreach (var item in fragmentAttributes.Where(
            att => att.FragmentHostViewType != null))
        {
            var fragment = GetFragmentByViewType(item.FragmentHostViewType);

            // if the fragment exists, and is on top, then use the current attribute 
            if (fragment?.IsVisible != true || fragment.View?.FindViewById(item.FragmentContentId) == null)
                continue;

            attribute = item;
            break;
        }

        return attribute;
    }

    [Obsolete("Busca interfaces de la vista.", true)]
    public override BasePresentationAttribute CreatePresentationAttribute(Type? viewModelType, Type? viewType)
    {
        ArgumentNullException.ThrowIfNull(viewModelType, nameof(viewModelType));

        if (viewType!.IsSubclassOf(typeof(DialogFragment)))
        {
            Logger.Log(LogLevel.Trace, "PresentationAttribute not found for {ViewName}. Assuming DialogFragment presentation", viewType.Name);
            return new DialogFragmentPresentationAttribute(enterAnimation: int.MinValue)
            {
                ViewType = viewType,
                ViewModelType = viewModelType
            };
        }

        if (viewType.IsSubclassOf(typeof(Fragment)))
        {
            Logger.Log(LogLevel.Trace, "PresentationAttribute not found for {ViewName}. Assuming Fragment presentation", viewType.Name);
            return new FragmentPresentationAttribute(GetCurrentActivityViewModelType(), global::Android.Resource.Id.Content)
            {
                ViewType = viewType,
                ViewModelType = viewModelType
            };
        }

        if (viewType.IsSubclassOf(typeof(Activity)))
        {
            Logger.Log(LogLevel.Trace, "PresentationAttribute not found for {ViewName}. Assuming Activity presentation", viewType.Name);
            return new ActivityPresentationAttribute
            {
                ViewType = viewType,
                ViewModelType = viewModelType
            };
        }

        throw new InvalidOperationException($"Don't know how to create a presentation attribute for type {viewType}");
    }

    public override Task<bool> ChangePresentation(CrossPresentationHint hint)
    {
        ArgumentNullException.ThrowIfNull(hint, nameof(hint));

        if (hint is CrossPagePresentationHint pagePresentationHint)
        {
            var result = ChangePagePresentation(pagePresentationHint);
            return Task.FromResult(result);
        }

        return base.ChangePresentation(hint);
    }

    [Obsolete]
    private bool ChangePagePresentation(CrossPagePresentationHint pagePresentationHint)
    {
        var request = new CrossViewModelRequest(pagePresentationHint.ViewModel);
        var attribute = GetPresentationAttribute(request);

        if (attribute is ViewPagerFragmentPresentationAttribute pagerFragmentAttribute)
        {
            var viewPager = FindViewPagerInFragmentPresentation(pagerFragmentAttribute);
            if (viewPager?.Adapter is MvxCachingFragmentStatePagerAdapter adapter)
            {
                var fragmentInfo = FindFragmentInfoFromAttribute(pagerFragmentAttribute, adapter);
                var index = adapter.FragmentsInfo.IndexOf(fragmentInfo!);
                if (index < 0)
                {
                    Logger.Log(LogLevel.Trace, "Did not find ViewPager index for {Fragment}, skipping presentation change...", pagerFragmentAttribute.Tag);
                    return true;
                }

                viewPager.SetCurrentItem(index, true);
                return true;
            }
        }

        return false;
    }

    [Obsolete]
    protected virtual ViewPager? FindViewPagerInFragmentPresentation(
        ViewPagerFragmentPresentationAttribute pagerFragmentAttribute)
    {
        ArgumentNullException.ThrowIfNull(pagerFragmentAttribute);

        ViewPager? viewPager = null;

        // check for a ViewPager inside a Fragment
        if (pagerFragmentAttribute.FragmentHostViewType != null)
        {
            var fragment = GetFragmentByViewType(pagerFragmentAttribute.FragmentHostViewType);
            viewPager = fragment?.View?.FindViewById<ViewPager>(pagerFragmentAttribute.ViewPagerResourceId);
        }

        // check for a ViewPager inside an Activity
        if (viewPager == null && pagerFragmentAttribute.ActivityHostViewModelType != null &&
            CurrentActivity.IsActivityAlive())
        {
            viewPager = CurrentActivity!.FindViewById<ViewPager>(pagerFragmentAttribute.ViewPagerResourceId);
        }

        return viewPager;
    }

    [Obsolete("Migrate to PressenterAction", true)]
    [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
    protected Type? GetCurrentActivityViewModelType()
    {
        Type? currentActivityType = null;
        if (CurrentActivity!.IsActivityAlive())
            currentActivityType = CurrentActivity!.GetType();

        if (currentActivityType == null)
            return null;

        Singleton<ViewsViewKeyContainerManager>.Instance.TryGetValue(currentActivityType, out var viewModelType);
        return viewModelType;
    }

    #region Show implementations
    [Obsolete("Migrate to PressenterAction", true)]
    protected virtual Task<bool> ShowActivity(
        Type view,
        ActivityPresentationAttribute attribute,
        CrossViewModelRequest request)
    {
        ArgumentNullException.ThrowIfNull(view);
        ArgumentNullException.ThrowIfNull(attribute);
        ArgumentNullException.ThrowIfNull(request);

        var intent = CreateIntentForRequest(request);
        if (intent == null)
            return Task.FromResult(false);

        if (attribute.Extras != null)
            intent.PutExtras(attribute.Extras);

        ShowIntent(intent, CreateActivityTransitionOptions(intent, attribute, request));
        return Task.FromResult(true);
    }

    [Obsolete("Migrate to PressenterAction", true)]
    protected virtual Bundle CreateActivityTransitionOptions(
        Intent intent, ActivityPresentationAttribute attribute, CrossViewModelRequest request)
    {
        ArgumentNullException.ThrowIfNull(attribute);
        ArgumentNullException.ThrowIfNull(request);


        ArgumentNullException.ThrowIfNull(intent, nameof(intent));

        var bundle = Bundle.Empty!;

        if (!(CurrentActivity is IMvxAndroidSharedElements sharedElementsActivity))
        {
            return bundle;
        }

        if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
        {
            Logger.Log(LogLevel.Warning, "Shared element transition requires Android v21+");
            return bundle;
        }

        if (CurrentActivity.IsActivityAlive())
        {
            var (elements, transitionElementPairs) =
                GetTransitionElements(attribute, request, sharedElementsActivity);

            if (transitionElementPairs.Count == 0)
            {
                Logger.Log(LogLevel.Warning, "No transition elements are provided");
                return bundle;
            }

            var transitionElementsBundle = CreateTransitionElementsBundle(intent, transitionElementPairs, elements);
            if (transitionElementsBundle != null)
                return transitionElementsBundle;
        }

        return bundle;
    }

    [Obsolete("Migrate to PressenterAction", true)]
    private Bundle? CreateTransitionElementsBundle(
        Intent intent, IEnumerable<Pair> transitionElementPairs, IEnumerable<string> elements)
    {
        var activityOptions = ActivityOptions.MakeSceneTransitionAnimation(
            CurrentActivity, transitionElementPairs.ToArray());
        if (activityOptions == null)
            return null;

        intent.PutExtra(SharedElementsBundleKey, string.Join("|", elements));
        var activityOptionsBundle = activityOptions.ToBundle();
        return activityOptionsBundle;
    }

    [Obsolete("Migrate to PressenterAction", true)]
    private (List<string> elements, List<Pair> transitionElementPairs) GetTransitionElements(
        BasePresentationAttribute attribute, CrossViewModelRequest request,
        IMvxAndroidSharedElements sharedElementsActivity)
    {
        var elements = new List<string>();
        var transitionElementPairs = new List<Pair>();

        foreach (var (key, value) in sharedElementsActivity.FetchSharedElementsToAnimate(attribute, request))
        {
            var transitionName = value.GetTransitionNameSupport();
            if (!string.IsNullOrEmpty(transitionName))
            {
                var pair = Pair.Create(value, transitionName);
                if (pair != null)
                {
                    transitionElementPairs.Add(pair);
                    elements.Add($"{key}:{transitionName}");
                }
            }
            else
            {
                Logger.Log(LogLevel.Warning, "A XML transitionName is required in order to transition a control when navigating");
            }
        }

        return (elements, transitionElementPairs);
    }

    [Obsolete("Migrada a AndroidPressenterAction", true)]
    protected virtual Intent? CreateIntentForRequest(CrossViewModelRequest? request)
    {
        if (request is CrossViewModelInstanceRequest viewModelInstanceRequest)
        {
            var intentWithKey = _viewModelRequestTranslator.GetIntentWithKeyFor(
                viewModelInstanceRequest.ViewModelInstance,
                viewModelInstanceRequest
            );

            return intentWithKey.intent;
        }

        return _viewModelRequestTranslator.GetIntentFor(request!);
    }

    [Obsolete("Migrada a AndroidPressenterAction", true)]
    protected virtual void ShowIntent(Intent intent, Bundle? bundle)
    {
        ArgumentNullException.ThrowIfNull(intent, nameof(intent));

        var activity = CurrentActivity;
        if (activity!.IsActivityDead())
        {
            Logger.Log(LogLevel.Error, "Cannot Resolve current top activity. Creating new activity from Application Context");
            intent.AddFlags(ActivityFlags.NewTask);
            StartActivity(Application.Context, intent, bundle);
            return;
        }

        StartActivity(activity!, intent, bundle);
    }

    [Obsolete("Migrada a AndroidPressenterAction", true)]
    private static void StartActivity(Context context, Intent intent, Bundle? bundle)
    {
        if (bundle != null)
        {
            context.StartActivity(intent, bundle);
        }
        else
        {
            context.StartActivity(intent);
        }
    }

    protected virtual void ShowHostActivity(FragmentPresentationAttribute attribute)
    {
        ArgumentNullException.ThrowIfNull(attribute);

        if (attribute.ActivityHostViewModelType == null)
            throw new ArgumentException("ActivityHostViewModelType not set on attribute");

        var viewType = base.ViewsContainer?.GetViewType(attribute.ActivityHostViewModelType);
        if (viewType?.IsSubclassOf(typeof(Activity)) != true)
            throw new CrossException("The host activity doesn't inherit Activity");

        var hostViewModelRequest = CrossViewModelRequest.GetDefaultRequest(attribute.ActivityHostViewModelType);
        if (PendingRequest != null)
            hostViewModelRequest.PresentationValues = PendingRequest.PresentationValues;

        Show(hostViewModelRequest);
    }

    [Obsolete("Migrate to PressenterAction", true)]
    protected virtual Task<bool> ShowFragment(
        Type view,
        FragmentPresentationAttribute attribute,
        CrossViewModelRequest request)
    {
        ArgumentNullException.ThrowIfNull(view);
        ArgumentNullException.ThrowIfNull(attribute);
        ArgumentNullException.ThrowIfNull(request);

        // if attribute has a Fragment Host, then show it as nested and return
        if (attribute.FragmentHostViewType != null)
        {
            ShowNestedFragment(view, attribute, request);

            return Task.FromResult(true);
        }

        // if there is no Activity host associated, assume is the current activity
        if (attribute.ActivityHostViewModelType == null)
            attribute.ActivityHostViewModelType = GetCurrentActivityViewModelType();

        var currentHostViewModelType = GetCurrentActivityViewModelType();
        if (attribute.ActivityHostViewModelType != currentHostViewModelType)
        {
            Logger.LogWarning("Activity host with ViewModelType {ActivityHostViewModelType} is not CurrentTopActivity. Showing Activity before showing Fragment for {ViewModelType}",
                attribute.ActivityHostViewModelType, attribute.ViewModelType);
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

    [Obsolete("Migrate to PressenterAction", true)]
    protected virtual void ShowNestedFragment(
        Type view,
        FragmentPresentationAttribute attribute,
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

    [Obsolete("Migrate to PressenterAction", true)]
    protected virtual void PerformShowFragmentTransaction(
        FragmentManager fragmentManager,
        FragmentPresentationAttribute attribute,
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
#pragma warning disable CA2000 // Dispose objects before losing scope
        var bundle = new Bundle();
#pragma warning restore CA2000 // Dispose objects before losing scope
        var serializedRequest = NavigationSerializer?.Serializer.SerializeObject(request);
        if (!string.IsNullOrEmpty(serializedRequest))
            bundle.PutString(ViewModelRequestBundleKey, serializedRequest);

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

    [Obsolete("Migrate to PressenterAction", true)]
    protected virtual void OnBeforeFragmentChanging(
        FragmentTransaction fragmentTransaction,
        Fragment fragment,
        FragmentPresentationAttribute attribute,
        CrossViewModelRequest request)
    {
        ArgumentNullException.ThrowIfNull(fragmentTransaction, nameof(fragmentTransaction));
        ArgumentNullException.ThrowIfNull(fragment, nameof(fragment));
        ArgumentNullException.ThrowIfNull(attribute);
        ArgumentNullException.ThrowIfNull(request);

        if (CurrentActivity.IsActivityAlive() && CurrentActivity is IMvxAndroidSharedElements sharedElementsActivity)
        {
            var elements = new List<string>();

            foreach (var (key, value) in sharedElementsActivity.FetchSharedElementsToAnimate(attribute, request))
            {
                var transitionName = value.GetTransitionNameSupport();
                if (!string.IsNullOrEmpty(transitionName))
                {
                    fragmentTransaction.AddSharedElement(value, transitionName);
                    elements.Add($"{key}:{transitionName}");
                }
                else
                {
                    Logger.Log(LogLevel.Warning, "A XML transitionName is required in order to transition a control when navigating");
                }
            }

            if (elements.Count > 0)
                fragment.Arguments?.PutString(SharedElementsBundleKey, string.Join("|", elements));
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

    [Obsolete("Migrate to PressenterAction", true)]
    protected virtual void OnFragmentChanged(FragmentTransaction? fragmentTransaction, Fragment? fragment, FragmentPresentationAttribute? attribute, CrossViewModelRequest? request)
    {
    }

    [Obsolete("Migrate to PressenterAction", true)]
    protected virtual void OnFragmentChanging(FragmentTransaction? fragmentTransaction, Fragment? fragment, FragmentPresentationAttribute? attribute, CrossViewModelRequest? request)
    {
    }

    protected virtual void OnFragmentPopped(FragmentTransaction? fragmentTransaction, Fragment? fragment, FragmentPresentationAttribute? attribute)
    {
    }

    [Obsolete("Migrate to PressenterAction", true)]
    protected virtual Task<bool> ShowDialogFragment(
        Type view,
        DialogFragmentPresentationAttribute attribute,
        CrossViewModelRequest request)
    {
        ArgumentNullException.ThrowIfNull(view);
        ArgumentNullException.ThrowIfNull(attribute);
        ArgumentNullException.ThrowIfNull(request);

        if (CurrentActivity == null)
            throw new InvalidOperationException("CurrentActivity is null");

        if (CurrentFragmentManager == null)
            throw new InvalidOperationException("CurrentFragmentManager is null. Cannot create Fragment Transaction.");

        if (attribute.ViewType == null)
            throw new InvalidOperationException($"{nameof(DialogFragmentPresentationAttribute)}.ViewType is null");

        var fragmentName = attribute.Tag ?? attribute.ViewType.FragmentJavaName();
        IMvxFragmentView mvxFragmentView = CreateFragment(CurrentActivity.SupportFragmentManager, attribute, attribute.ViewType);
        var dialog = (DialogFragment)mvxFragmentView;

        // MvxNavigationService provides an already instantiated ViewModel here,
        // therefore just assign it
        if (request is CrossViewModelInstanceRequest instanceRequest)
        {
            mvxFragmentView.ViewModel = instanceRequest.ViewModelInstance;
        }
        else
        {
            mvxFragmentView.LoadViewModelFrom(request);
        }

        dialog.Cancelable = attribute.Cancelable;

        var ft = CurrentFragmentManager.BeginTransaction();

        OnBeforeFragmentChanging(ft, dialog, attribute, request);

        ft.SetReorderingAllowed(attribute.AllowReordering);

        if (attribute.AddToBackStack)
            ft.AddToBackStack(fragmentName);

        OnFragmentChanging(ft, dialog, attribute, request);

        if (attribute.SetAsPrimaryFragment)
            ft.SetPrimaryNavigationFragment(dialog);

        dialog.Show(ft, fragmentName);

        OnFragmentChanged(ft, dialog, attribute, request);
        return Task.FromResult(true);
    }

    [Obsolete("Migrate to PressenterAction", true)]
    protected virtual Task<bool> ShowViewPagerFragment(
        Type view,
        ViewPagerFragmentPresentationAttribute attribute,
        CrossViewModelRequest request)
    {
        ArgumentNullException.ThrowIfNull(view);
        ArgumentNullException.ThrowIfNull(attribute);
        ArgumentNullException.ThrowIfNull(request);

        // if the attribute doesn't supply any host, assume current activity!
        if (attribute.FragmentHostViewType == null && attribute.ActivityHostViewModelType == null)
            attribute.ActivityHostViewModelType = GetCurrentActivityViewModelType();

        ViewPager? viewPager = null;
        FragmentManager? fragmentManager = null;

        // check for a ViewPager inside a Fragment
        if (attribute.FragmentHostViewType != null)
        {
            var fragment = GetFragmentByViewType(attribute.FragmentHostViewType);
            if (fragment == null)
                throw new CrossException("Fragment not found", attribute.FragmentHostViewType.Name);

            if (fragment.View == null)
            {
                throw new CrossException("Fragment.View is null. Please consider calling Navigate later in your code",
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
                return Task.FromResult(false);
            }

            if (CurrentActivity.IsActivityAlive())
                viewPager = CurrentActivity!.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
            fragmentManager = CurrentFragmentManager;
        }

        // no more cases to check. Just throw if ViewPager wasn't found
        if (viewPager == null)
            throw new CrossException("ViewPager not found");

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

        return Task.FromResult(true);
    }

    [Obsolete("Migrate to PressenterAction", true)]
    protected virtual async Task<bool> ShowTabLayout(
        Type view,
        TabLayoutPresentationAttribute attribute,
        CrossViewModelRequest request)
    {
        ArgumentNullException.ThrowIfNull(view);
        ArgumentNullException.ThrowIfNull(attribute);
        ArgumentNullException.ThrowIfNull(request);

        var showViewPagerFragment = await ShowViewPagerFragment(view, attribute, request).ConfigureAwait(true);
        if (!showViewPagerFragment)
            return false;

        ViewPager? viewPager = null;
        TabLayout? tabLayout = null;

        // check for a ViewPager inside a Fragment
        if (attribute.FragmentHostViewType != null)
        {
            var fragment = GetFragmentByViewType(attribute.FragmentHostViewType);

            viewPager = fragment?.View?.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
            tabLayout = fragment?.View?.FindViewById<TabLayout>(attribute.TabLayoutResourceId);
        }

        // check for a ViewPager inside an Activity
        if (CurrentActivity.IsActivityAlive() && attribute.ActivityHostViewModelType != null)
        {
            viewPager = CurrentActivity?.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
            tabLayout = CurrentActivity?.FindViewById<TabLayout>(attribute.TabLayoutResourceId);
        }

        if (viewPager == null || tabLayout == null)
            throw new CrossException("ViewPager or TabLayout not found");

        tabLayout.SetupWithViewPager(viewPager);
        return true;
    }

    #endregion

    #region Close implementations
    [Obsolete("Migrate to PressenterAction", true)]
    protected virtual Task<bool> CloseActivity(ICrossViewModel viewModel, ActivityPresentationAttribute? attribute)
    {
        var currentView = CurrentActivity as ICrossView;

        if (currentView == null)
        {
            Logger.Log(LogLevel.Warning, "Ignoring close for viewmodel - rootframe has no current page");
            return Task.FromResult(false);
        }

        if (currentView.ViewModel != viewModel)
        {
            Logger.Log(LogLevel.Warning, "Ignoring close for viewmodel - rootframe's current page is not the view for the requested viewmodel");
            return Task.FromResult(false);
        }

        // don't kill the dead
        if (CurrentActivity.IsActivityAlive())
            CurrentActivity!.Finish();

        return Task.FromResult(true);
    }

    [Obsolete("Migrate to PressenterAction", true)]
    protected virtual Task<bool> CloseFragmentDialog(
        ICrossViewModel viewModel, DialogFragmentPresentationAttribute attribute)
    {
        ArgumentNullException.ThrowIfNull(attribute);

        string tag = attribute.Tag ?? attribute.ViewType.FragmentJavaName();
        var toClose = CurrentFragmentManager?.FindFragmentByTag(tag);
        if (toClose is DialogFragment dialog)
        {
            dialog.DismissAllowingStateLoss();
            //return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    protected virtual bool CloseFragments()
    {
        try
        {
            CurrentFragmentManager?.PopBackStackImmediate();
        }
#pragma warning disable CA1031 // Do not catch general exception types
        catch (System.Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
        {
            Logger.Log(LogLevel.Warning, ex, "Cannot close any fragments");
        }
        return true;
    }

    [Obsolete("Migrate to PressenterAction", true)]
    protected virtual Task<bool> CloseFragment(
        ICrossViewModel viewModel, FragmentPresentationAttribute attribute)
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

    [Obsolete("Migrate to PressenterAction", true)]
    protected virtual bool TryPerformCloseFragmentTransaction(
        FragmentManager fragmentManager,
        FragmentPresentationAttribute fragmentAttribute)
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
#pragma warning disable CA1031 // Do not catch general exception types
        catch (System.Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
        {
            Logger.Log(LogLevel.Error, ex, "Cannot close fragment transaction");
            return false;
        }

        return false;
    }

    [Obsolete("Migrate to PressenterAction", true)]
    private void PopFragment(FragmentManager fragmentManager, FragmentPresentationAttribute fragmentAttribute,
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

    [Obsolete("Migrate to PressenterAction", true)]
    private void PopOnBackstackEntries(
        string fragmentName, FragmentManager fragmentManager, FragmentPresentationAttribute fragmentAttribute)
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

    [Obsolete("Migrate to PressenterAction", true)]
    protected virtual Task<bool> CloseViewPagerFragment(
        ICrossViewModel? viewModel,
        ViewPagerFragmentPresentationAttribute attribute)
    {
        ArgumentNullException.ThrowIfNull(attribute);

        ViewPager? viewPager = null;
        FragmentManager? fragmentManager;

        if (attribute.FragmentHostViewType != null)
        {
            var fragment = GetFragmentByViewType(attribute.FragmentHostViewType);
            if (fragment == null)
                throw new CrossException("Fragment not found", attribute.FragmentHostViewType.Name);

            viewPager = fragment.View?.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
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

                if (fragment != null)
                    ft.Remove(fragment);

                ft.CommitAllowingStateLoss();
                adapter.NotifyDataSetChanged();

                OnFragmentPopped(ft, fragment, attribute);
                return Task.FromResult(true);
            }
        }

        return Task.FromResult(false);
    }

    [Obsolete("Migrate to PressenterAction", true)]
    protected virtual MvxViewPagerFragmentInfo? FindFragmentInfoFromAttribute(
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
    #endregion

    [Obsolete("Migrate to PressenterAction", true)]
    protected virtual IMvxFragmentView CreateFragment(
        FragmentManager fragmentManager,
        BasePresentationAttribute attribute,
        Type fragmentType)
    {
        ArgumentNullException.ThrowIfNull(attribute);
        ArgumentNullException.ThrowIfNull(fragmentManager);
        ArgumentNullException.ThrowIfNull(fragmentType);

        try
        {
            var fragmentClass = Class.FromType(fragmentType);
            var fragment = (IMvxFragmentView)fragmentManager.FragmentFactory.Instantiate(
                fragmentClass.ClassLoader!, fragmentClass.Name);
            return fragment;
        }
        catch (System.Exception ex)
        {
            throw new CrossException(ex, $"Cannot create Fragment '{fragmentType.Name}'");
        }
    }

    [Obsolete("Migrate to PressenterAction", true)]
    protected virtual Fragment? GetFragmentByViewType(Type? type)
    {
        if (type == null)
            return null;

        if (CurrentFragmentManager == null)
            return null;

        var fragmentName = type.FragmentJavaName();
        var fragment = CurrentFragmentManager.FindFragmentByTag(fragmentName);

        if (fragment != null)
        {
            return fragment;
        }

        return FindFragmentInChildren(fragmentName, CurrentFragmentManager);
    }

    [Obsolete("Migrate to PressenterAction", true)]
    protected virtual Fragment? FindFragmentInChildren(string? fragmentName, FragmentManager? fragmentManager)
    {
        if (string.IsNullOrWhiteSpace(fragmentName))
        {
            return null;
        }

        if (fragmentManager == null)
        {
            return null;
        }

        foreach (var parentFragmentManager in fragmentManager.Fragments.Select(f => f.ChildFragmentManager))
        {
            // Let's try again finding it
            var fragment = parentFragmentManager.FindFragmentByTag(fragmentName);

            if (fragment == null)
            {
                // Re-loop for other fragments
                fragment = FindFragmentInChildren(fragmentName, parentFragmentManager);
            }

            // If we found the fragment let's return it!
            if (fragment != null)
            {
                return fragment;
            }
        }

        return null;
    }
}
