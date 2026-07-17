using Android.Content.PM;
using Android.Runtime;

namespace Nivaes.App.Cross.Droid
{
    [Activity(Name = "com.nivaes.InitializingAppView",
        Theme = "@style/AppTheme.initializing_app",
        LaunchMode = LaunchMode.SingleTop)]
    [Register("com.nivaes.InitializingAppView")]
    public sealed class InitializingAppView
        : CrossActivity<InitializingAppViewModel>
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            base.SetContentView(Resource.Layout.initializing_app);
        }
    }
}
