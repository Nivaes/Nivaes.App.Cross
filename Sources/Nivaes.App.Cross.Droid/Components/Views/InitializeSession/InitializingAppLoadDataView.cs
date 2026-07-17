using Android.Content.PM;
using Android.Runtime;

namespace Nivaes.App.Cross.Droid
{
    [Activity(Name = "com.nivaes.InitializingAppLoadDataView",
        Theme = "@style/AppTheme.initializing_app_load_data",
        LaunchMode = LaunchMode.SingleTop)]
    [Register("com.nivaes.InitializingAppLoadDataView")]
    public sealed class InitializingAppLoadDataView
        : CrossActivity<InitializingAppLoadDataViewModel>
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            base.SetContentView(Resource.Layout.initializing_app_load_data);
        }
    }
}
