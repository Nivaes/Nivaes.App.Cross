namespace Nivaes.App.Cross.Sample.Droid
{
    using Nivaes.App.Cross.Droid;

    [Activity(
        Label = "@string/app_name",
        Theme = "@style/AppTheme.Splash",
        MainLauncher = true,
        NoHistory = true)]
    public class SplashScreen : StartActivity
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreenView)
        {
        }
    }
}