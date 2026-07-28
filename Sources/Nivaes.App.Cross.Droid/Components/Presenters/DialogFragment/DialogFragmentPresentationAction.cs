using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using Microsoft.Extensions.Logging;
using Activity = AndroidX.AppCompat.App.AppCompatActivity;
using DialogFragment = AndroidX.Fragment.App.DialogFragment;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;
using FragmentTransaction = AndroidX.Fragment.App.FragmentTransaction;

namespace Nivaes.App.Cross.Droid
{
    public sealed class DialogFragmentPresentationAction
        : PressenterAction<DialogFragmentPresentationAttribute>, IFragmentPresentationAction
    {
        private IFragmentPresentationAction thisFragment => (IFragmentPresentationAction)this;

        public DialogFragmentPresentationAction(
                IPressenterActionContext context,
                ILogger<DialogFragmentPresentationAction> logger)
            : base(context, logger)
        {
        }

        protected override ValueTask<bool> ShowAction(DialogFragmentPresentationAttribute attribute, IViewModelRequest request)
        {
            if (base.Context.CurrentActivity == null)
                throw new InvalidOperationException("CurrentActivity is null");

            if (base.Context.CurrentFragmentManager == null)
                throw new InvalidOperationException("CurrentFragmentManager is null. Cannot create Fragment Transaction.");

            if (attribute.ViewType == null)
                throw new InvalidOperationException($"{nameof(DialogFragmentPresentationAttribute)}.ViewType is null");

            var fragmentName = attribute.Tag ?? attribute.ViewType.FragmentJavaName();
            IMvxFragmentView mvxFragmentView = thisFragment.CreateFragment(base.Context.CurrentActivity.SupportFragmentManager, attribute, attribute.ViewType);
            var dialog = (DialogFragment)mvxFragmentView;

            // MvxNavigationService provides an already instantiated ViewModel here,
            // therefore just assign it
            //if (request is CrossViewModelInstanceRequest instanceRequest)
            //{
            //    mvxFragmentView.ViewModel = instanceRequest.ViewModelInstance;
            //}
            //else
            //{
            //    mvxFragmentView.LoadViewModelFrom(request);
            //}
            mvxFragmentView.ViewModel = request.ViewModel;

            dialog.Cancelable = attribute.Cancelable;

            var ft = base.Context.CurrentFragmentManager.BeginTransaction();

            thisFragment.OnBeforeFragmentChanging(ft, dialog, attribute, request);

            ft.SetReorderingAllowed(attribute.AllowReordering);

            if (attribute.AddToBackStack)
                ft.AddToBackStack(fragmentName);

            thisFragment.OnFragmentChanging(ft, dialog, attribute, request);

            if (attribute.SetAsPrimaryFragment)
                ft.SetPrimaryNavigationFragment(dialog);

            dialog.Show(ft, fragmentName);

            OnFragmentChanged(ft, dialog, attribute, request);
            return ValueTask.FromResult(true);
        }

        protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, DialogFragmentPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(attribute);

            string tag = attribute.Tag ?? attribute.ViewType.FragmentJavaName();
            var toClose = base.Context.CurrentFragmentManager?.FindFragmentByTag(tag);
            if (toClose is DialogFragment dialog)
            {
                dialog.DismissAllowingStateLoss();
                return ValueTask.FromResult(true);
            }
            return ValueTask.FromResult(false);
        }
    }
}
