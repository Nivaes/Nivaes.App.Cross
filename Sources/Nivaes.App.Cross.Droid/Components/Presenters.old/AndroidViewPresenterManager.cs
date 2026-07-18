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

public class AndroidViewPresenterManager 
    : CrossViewPresenterManager, IAndroidViewPresenterManager
{
    public const string ViewModelRequestBundleKey = "__mvxViewModelRequest";
    public const string SharedElementsBundleKey = "__sharedElementsKey";

    private readonly IMvxAndroidCurrentTopActivity _androidCurrentTopActivity;
    private readonly IMvxAndroidActivityLifetimeListener _activityLifetimeListener;

    private readonly IMvxAndroidViewModelRequestTranslator _viewModelRequestTranslator;

    protected CrossViewModelRequest? PendingRequest { get; set; }

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

    [Obsolete("Migrate to PressenterAction", true)]
    public override BasePresentationAttribute GetPresentationAttribute(CrossViewModelRequest request)
    {
        var viewType = Singleton<ViewModelViewsKeyContainerManager>.Instance
                .GetValue(request.ViewModelType);

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

    [Obsolete("Migrate to PressenterAction", true)]
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

    [Obsolete("", true)]
    public override ValueTask<bool> ChangePresentation(CrossPresentationHint hint)
    {
        if (hint is CrossPagePresentationHint pagePresentationHint)
        {
            var result = ChangePagePresentation(pagePresentationHint);
            return ValueTask.FromResult(result);
        }

        return base.ChangePresentation(hint);
    }

    [Obsolete("", true)]
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

    [Obsolete("Migrate to PressenterAction", true)]
    protected virtual ViewPager? FindViewPagerInFragmentPresentation(
        ViewPagerFragmentPresentationAttribute pagerFragmentAttribute)
    {
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

    #region Show implementations
    protected virtual void ShowHostActivity(FragmentPresentationAttribute attribute)
    {
        if (attribute.ActivityHostViewModelType == null)
            throw new ArgumentException("ActivityHostViewModelType not set on attribute");

        var viewType = Singleton<ViewModelViewsKeyContainerManager>.Instance
                .GetValue(attribute.ActivityHostViewModelType);

        if (viewType?.IsSubclassOf(typeof(Activity)) != true)
            throw new AppException("The host activity doesn't inherit Activity");
        
        var hostViewModelRequest = CrossViewModelRequest.GetDefaultRequest(attribute.ActivityHostViewModelType);
        if (PendingRequest != null)
            hostViewModelRequest.PresentationValues = PendingRequest.PresentationValues;

        Show(hostViewModelRequest);
    }

  
    [Obsolete("Migrate to PressenterAction", true)]
    protected virtual Task<bool> ShowViewPagerFragment(
        Type view,
        ViewPagerFragmentPresentationAttribute attribute,
        CrossViewModelRequest request)
    {
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
                throw new AppException("Fragment not found", attribute.FragmentHostViewType.Name);

            if (fragment.View == null)
            {
                throw new AppException("Fragment.View is null. Please consider calling Navigate later in your code",
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
            throw new AppException("ViewPager not found");

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

    

    #endregion

    #region Close implementations
   

    [Obsolete("Migrate to PressenterAction", true)]
    protected virtual MvxViewPagerFragmentInfo? FindFragmentInfoFromAttribute(
        FragmentPresentationAttribute attribute,
        MvxCachingFragmentStatePagerAdapter adapter)
    {
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
