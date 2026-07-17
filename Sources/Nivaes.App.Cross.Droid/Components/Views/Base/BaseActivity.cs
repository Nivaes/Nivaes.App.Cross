using Android.Content.Res;
using Android.Views;
using Android.Views.InputMethods;
using AndroidX.DrawerLayout.Widget;
using Microsoft.Extensions.DependencyInjection;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace Nivaes.App.Cross.Droid
{
    public abstract class BaseActivity<TViewModel>
        : CrossActivity<TViewModel>, IMainActivity
        where TViewModel : class, IBaseViewModel
    {
        #region Properties
        public DrawerLayout? DrawerLayout { get; private set; }

        protected Toolbar? MainToolbar { get; private set; }

        protected abstract int LayoutId { get; }

        protected virtual bool ShowBackButton => false;

        #region ShowHamburgerMenu
        private bool mShowHamburgerMenu;

        protected virtual bool ShowHamburgerMenu => mShowHamburgerMenu;

        bool IMainActivity.ShowHamburgerMenu
        {
            get => mShowHamburgerMenu;
            set => mShowHamburgerMenu = value;
        }
        #endregion
        #endregion

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            base.SetContentView(LayoutId);

            //if (System.Diagnostics.Debugger.IsAttached)
            System.Diagnostics.Debugger.Break();

            //DrawerLayout = base.FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            //if (ShowBackButton && !ShowHamburgerMenu)
            //{
            //    var toolbar = FindViewById<androidx.appcompat.widget.Toolbar>(Resource.Id.main_toolbar);
            //    if (toolbar != null)
            //    {
            //        base.SetSupportActionBar(toolbar);
            //        base.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //        base.SupportActionBar.SetHomeButtonEnabled(true);
            //    }
            //}
        }

        void IMainActivity.StartActionBar()
        {
            var actionBar = base.SupportActionBar;

            if (actionBar != null)
            {
                actionBar.SetDisplayHomeAsUpEnabled(true);
                actionBar.SetDisplayShowTitleEnabled(true);

                if (ShowHamburgerMenu)
                {
                    actionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_hamburger_menu);

                    //if (System.Diagnostics.Debugger.IsAttached)
                    System.Diagnostics.Debugger.Break();

                    //DrawerLayout.DrawerOpened += (sender, e) =>
                    //{
                    //    HideSoftKeyboard();
                    //    actionBar.SetDisplayHomeAsUpEnabled(true);
                    //};

                    var shellService = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<IShellService>();
                    shellService.ShowMenu();
                }
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            //if (System.Diagnostics.Debugger.IsAttached)
            System.Diagnostics.Debugger.Break();

            //if (menu is MenuBuilder menuBuilder)
            //    menuBuilder.SetOptionalIconsVisible(true);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            ArgumentNullException.ThrowIfNull(item);

            //if (System.Diagnostics.Debugger.IsAttached)
            System.Diagnostics.Debugger.Break();

            //switch (item.ItemId)
            //{
            //    case Android.Resource.Id.Home:
            //        if (ShowHamburgerMenu)
            //            DrawerLayout.OpenDrawer(GravityCompat.Start);
            //        else
            //            OnBackPressed();

            //        return true;
            //}

            return base.OnOptionsItemSelected(item);
        }

        public void HideSoftKeyboard()
        {
            if (base.CurrentFocus == null) return;

            InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(base.CurrentFocus.WindowToken, 0);

            base.CurrentFocus.ClearFocus();
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            if (newConfig == null) throw new ArgumentNullException(nameof(newConfig));

            base.OnConfigurationChanged(newConfig);
#if DEBUG
            Android.Widget.Toast.MakeText(this, "Configuration change", Android.Widget.ToastLength.Short).Show();

            // Checks the orientation of the screen
            if (newConfig.Orientation == Android.Content.Res.Orientation.Landscape)
            {
                Android.Widget.Toast.MakeText(this, "landscape", Android.Widget.ToastLength.Short).Show();
            }
            else if (newConfig.Orientation == Android.Content.Res.Orientation.Portrait)
            {
                Android.Widget.Toast.MakeText(this, "portrait", Android.Widget.ToastLength.Short).Show();
            }
#endif
        }
    }
}
