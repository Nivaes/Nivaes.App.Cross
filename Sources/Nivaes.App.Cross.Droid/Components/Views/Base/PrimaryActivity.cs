using Android.Runtime;
using Android.Views;

namespace Nivaes.App.Cross.Droid
{

    [Activity(Name = "com.nivaes.app.PrimaryActivity",
              Label = "@string/application_name",
              Theme = "@style/AppTheme",
              WindowSoftInputMode = SoftInput.AdjustResize,
              NoHistory = false
              //WindowSoftInputMode = SoftInput.AdjustPan
              //ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation
              )]
    [Register("com.nivaes.app.PrimaryActivity")]
    public sealed class PrimaryActivity
        : BaseActivity<PrimaryViewModel>, IMainActivity
    {
        protected override int LayoutId => Resource.Layout.main_activity;

        protected override bool ShowHamburgerMenu => true;

        public override void OnBackPressed()
        {
        }
    }
}
