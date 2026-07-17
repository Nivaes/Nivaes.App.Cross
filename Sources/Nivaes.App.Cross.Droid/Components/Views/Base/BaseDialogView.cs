using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.View.Menu;
using Nivaes.App.Droid;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace Nivaes.App.Cross.Droid
{
    public abstract class BaseDialogView<TViewModel>
        : MvxDialogFragment<TViewModel>
         where TViewModel : IDialogViewModel
    {
        #region Properties
        protected abstract int LayoutId { get; }
        protected virtual int DialogToolbarId { get; } = Resource.Id.dialog_toolbar;
        protected virtual int MenuResourceId { get; } = 0;

        protected virtual DialogSizeType DialogSize => DialogSizeType.Dialog;
        protected bool IsFullScreen => DialogSize == DialogSizeType.FullScreen || (DialogSize == DialogSizeType.FullScreenMobile && !IsTablet);

        protected virtual NavigationIconType NavigationIconType { get; } = NavigationIconType.Back;

        protected string Title { get; set; }
        protected Toolbar DialogToolbar { get; private set; }

        protected bool IsTablet => base.Resources.GetBoolean(Resource.Boolean.isTablet);
        #endregion

        #region Constructors
        protected BaseDialogView()
            : base()
        {
            base.RetainInstance = true;
        }

        protected BaseDialogView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            base.RetainInstance = true;
        }
        #endregion

        public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(LayoutId, null);

            DialogToolbar = view.FindViewById<Toolbar>(DialogToolbarId);

            if (DialogToolbar != null)
            {
                DialogToolbar.Title = string.IsNullOrEmpty(Title) ? base.Context.GetString(Resource.String.application_name) : Title;

                switch (NavigationIconType)
                {
                    case NavigationIconType.Back:
                        DialogToolbar.SetNavigationIcon(Resource.Drawable.ic_arrow_back);
                        break;
                    case NavigationIconType.Cancel:
                        DialogToolbar.SetNavigationIcon(Resource.Drawable.ic_cancel);
                        break;
                }

                DialogToolbar.NavigationClick += (o, e) => DialogClose();

                if (MenuResourceId != 0)
                {
                    DialogToolbar.InflateMenu(MenuResourceId);
                    base.SetMenuVisibility(true);

                    DialogToolbar.MenuItemClick += (o, e) => OnOptionsItemSelected(e.Item);

                    if (DialogToolbar.Menu is MenuBuilder menuBuilder)
                        menuBuilder.SetOptionalIconsVisible(true);
                }
            }

            return view;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            Dialog dialog;

            if (IsFullScreen)
                dialog = new Dialog(base.Context, Resource.Style.AppTheme_Dialog_FullScreen);
            else
                dialog = new Dialog(base.Context, Resource.Style.AppTheme_Dialog);

            dialog.SetCanceledOnTouchOutside(false);
            dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            dialog.Window.SetSoftInputMode(SoftInput.AdjustResize);

            return dialog;
        }

        public override void OnResume()
        {
            if (IsFullScreen)
            {
                var windows = base.Dialog?.Window;

                windows.SetLayout(WindowManagerLayoutParams.MatchParent, WindowManagerLayoutParams.MatchParent);
            }
            //    var windows = Dialog.Window;

            //    var display = windows.WindowManager.DefaultDisplay;
            //    var size = new Point();
            //    display.GetSize(size);

            //    float px = TypedValue.ApplyDimension(ComplexUnitType.Dip, 480, base.Context.Resources.DisplayMetrics);
            //    if (size.X < px)
            //    {
            //        windows.SetLayout((int)(size.X * 0.9), (int)(size.Y * 0.8));
            //    }
            //    else
            //    {
            //        windows.SetLayout((int)(size.X * 0.75), (int)(size.Y * 0.75));
            //    }

            //    //windows.SetLayout(WindowManagerLayoutParams.WrapContent, WindowManagerLayoutParams.MatchParent);

            //    // Full-Screen
            //    //var attributes = windows.Attributes;
            //    //attributes.Width = WindowManagerLayoutParams.WrapContent;
            //    //attributes.Height = WindowManagerLayoutParams.MatchParent;
            //    //windows.Attributes = attributes;

            base.OnResume();
        }

        public override void OnDestroyView()
        {
            if (base.Dialog != null && base.RetainInstance)
            {
                base.Dialog.SetDismissMessage(null);
            }
            base.OnDestroyView();
        }

        protected virtual async void DialogClose()
        {
            if (ViewModel != null)
                await ViewModel.CloseCommand.ExecuteAsync().ConfigureAwait(false);
        }
    }
}
