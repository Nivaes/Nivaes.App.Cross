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
    internal interface IFragmentPresentationAction
    {
        IPressenterActionContext Context { get; }

        ICrossNavigationSerializer NavigationSerializer { get; }

        ILogger Logger { get; }

        IMvxFragmentView CreateFragment(
           FragmentManager fragmentManager,
           BasePresentationAttribute attribute,
           Type fragmentType)
        {
            try
            {
                var fragmentClass = Class.FromType(fragmentType);
                var fragment = (IMvxFragmentView)fragmentManager.FragmentFactory.Instantiate(
                    fragmentClass.ClassLoader!, fragmentClass.Name);
                return fragment;
            }
            catch (System.Exception ex)
            {
                throw new AppException(ex, $"Cannot create Fragment '{fragmentType.Name}'");
            }
        }

        void ShowNestedFragment(
            Type viewType,
            FragmentPresentationAttribute attribute,
            ViewModelRequest request)
        {
            // current implementation only supports one level of nesting 

            var fragmentHost = GetFragmentByViewType(attribute.FragmentHostViewType);
            if (fragmentHost == null)
                throw new InvalidOperationException($"Fragment host not found when trying to show View {viewType.Name} as Nested Fragment");

            if (!fragmentHost.IsVisible)
                Logger.Log(LogLevel.Warning, $"Fragment host is not visible when trying to show View {viewType.Name} as Nested Fragment");

            PerformShowFragmentTransaction(fragmentHost.ChildFragmentManager, attribute, request);
        }

        Fragment? GetFragmentByViewType(Type? type)
        {
            if (type == null)
                return null;

            if (Context.CurrentFragmentManager == null)
                return null;

            var fragmentName = type.FragmentJavaName();
            var fragment = Context.CurrentFragmentManager.FindFragmentByTag(fragmentName);

            if (fragment != null)
            {
                return fragment;
            }

            return FindFragmentInChildren(fragmentName, Context.CurrentFragmentManager);
        }

        void PerformShowFragmentTransaction(
            FragmentManager fragmentManager,
            FragmentPresentationAttribute attribute,
            ViewModelRequest request)
        {
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
                throw new AppException($"Fragment {fragmentName} is null. Cannot perform Fragment Transaction.");

            // MvxNavigationService provides an already instantiated ViewModel here
            //if (request is CrossViewModelInstanceRequest instanceRequest)
            //{
            //    fragmentView!.ViewModel = instanceRequest.ViewModelInstance;
            //}
            fragmentView!.ViewModel = request.ViewModel;

            // save MvxViewModelRequest in the Fragment's Arguments
            var bundle = new Bundle();
            var serializedRequest = NavigationSerializer.Serializer.SerializeObject(request);
            if (!string.IsNullOrEmpty(serializedRequest))
                bundle.PutString(AndroidViewPresenterManager.ViewModelRequestBundleKey, serializedRequest);

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

        Fragment? FindFragmentInChildren(string? fragmentName, FragmentManager? fragmentManager)
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

        void OnFragmentPopped(FragmentTransaction? fragmentTransaction, Fragment? fragment, FragmentPresentationAttribute? attribute)
        {
        }

        void OnBeforeFragmentChanging(
           FragmentTransaction fragmentTransaction,
           Fragment fragment,
           FragmentPresentationAttribute attribute,
           ViewModelRequest request)
        {
            if (Context.CurrentActivity.IsActivityAlive() && Context.CurrentActivity is IMvxAndroidSharedElements sharedElementsActivity)
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

        void OnFragmentChanging(FragmentTransaction? fragmentTransaction, Fragment? fragment, FragmentPresentationAttribute? attribute, ViewModelRequest? request)
        {
        }

        void OnFragmentChanged(FragmentTransaction? fragmentTransaction, Fragment? fragment, FragmentPresentationAttribute? attribute, ViewModelRequest? request);
    }
}
