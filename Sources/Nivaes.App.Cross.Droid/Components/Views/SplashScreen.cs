namespace Nivaes.App.Cross.Droid
{
    [Activity(Name = "com.nivaes.SplashScreen"
        , Label = "@string/application_name"
        , MainLauncher = true
        , Icon = "@drawable/ic_launcher"
        , Theme = "@style/AppTheme.Splash"
        , NoHistory = true)]
    public class SplashScreen
        : StartActivity
    {
        public SplashScreen()
            : base(Resource.Layout.splash_screen)
        {
        }
    }
}
