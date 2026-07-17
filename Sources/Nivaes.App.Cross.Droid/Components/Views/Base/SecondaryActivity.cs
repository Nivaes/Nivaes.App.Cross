namespace Nivaes.App.Cross.Droid
{
    using Android.App;
    using Android.Runtime;
    using Android.Views;

    [Activity(Name = "com.nivaes.app.SecondaryActivity",
              Label = "@string/application_name",
              Theme = "@style/AppTheme",
              WindowSoftInputMode = SoftInput.AdjustResize,
              NoHistory = false
              //WindowSoftInputMode = SoftInput.AdjustPan
              //ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation
              )]
    [Register("com.nivaes.app.SecondaryActivity")]
    public sealed class SecondaryActivity
        : BaseActivity<SecondaryViewModel>, IMainActivity
    {
        protected override int LayoutId => Resource.Layout.main_activity;

        protected override bool ShowHamburgerMenu => false;
    }
}
