using Microsoft.Extensions.Logging;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;

namespace Nivaes.App.Cross.Droid
{
    public sealed class FragmentPresentationAction
        : PressenterAction<FragmentPresentationAttribute>, IFragmentPresentationAction
    {
        private IFragmentPresentationAction thisFragment => (IFragmentPresentationAction)this;

        public FragmentPresentationAction(
                IPressenterActionContext context,
                ILogger<FragmentPresentationAttribute> logger)
            : base(context, logger)
        {
        }

        protected override ValueTask<bool> ShowAction(Type viewType, FragmentPresentationAttribute attribute, IViewModelRequest request)
        {
            var fragmentAttribute = (FragmentPresentationAttribute)attribute;
            // if attribute has a Fragment Host, then show it as nested and return
            if (fragmentAttribute.FragmentHostViewType != null)
            {
                thisFragment.ShowNestedFragment(viewType, fragmentAttribute, request);

                return ValueTask.FromResult(true);
            }

            // if there is no Activity host associated, assume is the current activity
            if (fragmentAttribute.ActivityHostViewModelType == null)
                fragmentAttribute.ActivityHostViewModelType = GetCurrentActivityViewModelType();

            var currentHostViewModelType = GetCurrentActivityViewModelType();
            if (fragmentAttribute.ActivityHostViewModelType != currentHostViewModelType)
            {
                Logger.LogWarning("Activity host with ViewModelType {ActivityHostViewModelType} is not CurrentTopActivity. Showing Activity before showing Fragment for {ViewModelType}",
                    fragmentAttribute.ActivityHostViewModelType, attribute.ViewModelType);
                PendingRequest = request;
                ShowHostActivity(attribute);
            }
            else if (base.Context.CurrentActivity!.IsActivityAlive())
            {
                if (base.Context.CurrentActivity!.FindViewById(attribute.FragmentContentId) == null)
                    throw new InvalidOperationException("FrameLayout to show Fragment not found");

                thisFragment.PerformShowFragmentTransaction(base.Context.CurrentActivity.SupportFragmentManager, attribute, request);
            }
            return ValueTask.FromResult(true);
        }

        protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, FragmentPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(attribute);

            // try to close nested fragment first
            if (attribute.FragmentHostViewType != null)
            {
                var fragmentHost = thisFragment.GetFragmentByViewType(attribute.FragmentHostViewType);
                if (fragmentHost != null
                    && TryPerformCloseFragmentTransaction(fragmentHost.ChildFragmentManager, attribute))
                    return ValueTask.FromResult(true);
            }

            // Close fragment. If it isn't successful, then close the current Activity
            if (base.Context.CurrentFragmentManager != null && TryPerformCloseFragmentTransaction(base.Context.CurrentFragmentManager, attribute))
            {
                return ValueTask.FromResult(true);
            }

            if (base.Context.CurrentActivity.IsActivityAlive())
            {
                base.Context.CurrentActivity!.Finish();
                return ValueTask.FromResult(true);
            }

            return ValueTask.FromResult(false);
        }

        private bool TryPerformCloseFragmentTransaction(
            FragmentManager fragmentManager,
            FragmentPresentationAttribute fragmentAttribute)
        {
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
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "Cannot close fragment transaction");
                return false;
            }

            return false;
        }

        private void PopFragment(FragmentManager fragmentManager,
                FragmentPresentationAttribute fragmentAttribute,
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

            thisFragment.OnFragmentPopped(ft, fragmentToPop, fragmentAttribute);
        }

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

            thisFragment.OnFragmentPopped(null, null, fragmentAttribute);
        }
    }
}
