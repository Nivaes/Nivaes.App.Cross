using AndroidX.DrawerLayout.Widget;
using ActionBar = AndroidX.AppCompat.App.ActionBar;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace Nivaes.App.Cross.Droid
{
    internal interface IMainActivity
    {
        DrawerLayout DrawerLayout { get; }

        void SetSupportActionBar(Toolbar toolbar);

        ActionBar SupportActionBar { get; }

        //void HideSoftKeyboard();

        bool ShowHamburgerMenu { get; set; }

        void StartActionBar();
    }
}
