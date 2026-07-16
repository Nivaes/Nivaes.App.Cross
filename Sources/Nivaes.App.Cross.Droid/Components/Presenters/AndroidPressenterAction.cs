using System.Diagnostics.CodeAnalysis;
using Java.Lang;
using Microsoft.Extensions.Logging;
using Activity = AndroidX.AppCompat.App.AppCompatActivity;
using DialogFragment = AndroidX.Fragment.App.DialogFragment;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;
using FragmentTransaction = AndroidX.Fragment.App.FragmentTransaction;

namespace Nivaes.App.Cross.Droid
{
    public abstract class AndroidPressenterAction<TPressenterAttribute>
        : PressenterAction<TPressenterAttribute>
        where TPressenterAttribute : IPresentationAttribute
    {
        #region Properties
        protected CrossViewModelRequest? PendingRequest { get; set; }

        private readonly IMvxAndroidCurrentTopActivity _androidCurrentTopActivity;

        protected Activity CurrentActivity => _androidCurrentTopActivity.Activity as Activity;

        protected virtual FragmentManager? CurrentFragmentManager
        {
            get
            {
                if (CurrentActivity.IsActivityDead())
                    return null;

                return CurrentActivity!.SupportFragmentManager;
            }
        }
        #endregion

        #region Constructor
        protected AndroidPressenterAction(
                ICrossViewsContainer viewsContainer,
                IMvxAndroidCurrentTopActivity androidCurrentTopActivity,
                ILogger logger)
            : base(viewsContainer, logger)
        {
            _androidCurrentTopActivity = androidCurrentTopActivity;
        }
        #endregion

        protected Fragment? GetFragmentByViewType(Type? type)
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

        private Fragment? FindFragmentInChildren(string? fragmentName, FragmentManager? fragmentManager)
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

        protected IMvxFragmentView CreateFragment(
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

        protected override BasePresentationAttribute CreatePresentationAttribute(Type? viewModelType, Type? viewType)
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
                Logger.LogTrace("PresentationAttribute not found for {ViewName}. Assuming Fragment presentation", viewType.Name);
                return new FragmentPresentationAttribute(GetCurrentActivityViewModelType(), global::Android.Resource.Id.Content)
                {
                    ViewType = viewType,
                    ViewModelType = viewModelType
                };
            }

            if (viewType.IsSubclassOf(typeof(Activity)))
            {
                Logger.LogTrace("PresentationAttribute not found for {ViewName}. Assuming Activity presentation", viewType.Name);
                return new ActivityPresentationAttribute
                {
                    ViewType = viewType,
                    ViewModelType = viewModelType
                };
            }

            throw new InvalidOperationException($"Don't know how to create a presentation attribute for type {viewType}");
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

        protected virtual void ShowHostActivity(FragmentPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(attribute);

            if (attribute.ActivityHostViewModelType == null)
                throw new ArgumentException("ActivityHostViewModelType not set on attribute");

            var viewType = ViewsContainer?.GetViewType(attribute.ActivityHostViewModelType);
            if (viewType?.IsSubclassOf(typeof(Activity)) != true)
                throw new CrossException("The host activity doesn't inherit Activity");

            var hostViewModelRequest = CrossViewModelRequest.GetDefaultRequest(attribute.ActivityHostViewModelType);
            if (PendingRequest != null)
                hostViewModelRequest.PresentationValues = PendingRequest.PresentationValues;

            Show(hostViewModelRequest);
        }
      

        protected virtual void OnFragmentPopped(FragmentTransaction? fragmentTransaction, Fragment? fragment, FragmentPresentationAttribute? attribute)
        {
        }

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
                    fragment.Arguments?.PutString(AndroidViewPresenterManager.SharedElementsBundleKey, string.Join("|", elements));
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

        protected virtual void OnFragmentChanging(FragmentTransaction? fragmentTransaction, Fragment? fragment, FragmentPresentationAttribute? attribute, CrossViewModelRequest? request)
        {
        }

        protected virtual void OnFragmentChanged(FragmentTransaction? fragmentTransaction, Fragment? fragment, FragmentPresentationAttribute? attribute, CrossViewModelRequest? request)
        {
        }
    }
}
