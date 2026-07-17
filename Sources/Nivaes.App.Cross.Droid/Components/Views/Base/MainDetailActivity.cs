namespace Nivaes.App.Cross.Droid
{
    using Android.App;
    using Android.Runtime;
    using Android.Views;

    [Activity(Name = "com.nivaes.app.MainDetailActivity",
              Label = "@string/application_name",
              Theme = "@style/AppTheme",
              WindowSoftInputMode = SoftInput.AdjustResize
              )]
    [Register("com.nivaes.app.MainDetailActivity")]
    public sealed class MainDetailActivity
        : BaseDetailActivity<MainDetailViewModel>
    {
        #region Properties
        protected override int LayoutId => Resource.Layout.main_detail_activity;
        #endregion

        public override void OnBackPressed()
        {
            base.OnBackPressed();
        }
    }
}
