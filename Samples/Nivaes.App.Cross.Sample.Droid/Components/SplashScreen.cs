using System.Diagnostics.CodeAnalysis;
using Android.Content.PM;
using Android.Runtime;
using Nivaes.App.Cross.Droid;

namespace Nivaes.App.Cross.Sample.Droid;

[Activity(
    Label = "Nivaes.App.Droid"
    , MainLauncher = true
    , Icon = "@mipmap/icon"
    , Theme = "@style/AppTheme.Splash"
    , NoHistory = true
    , ScreenOrientation = ScreenOrientation.Portrait)]
[Register("nivaes.cross.sample.SplashScreen")]
public sealed class SplashScreen 
    : StartActivity<StartViewModel>
{
    public SplashScreen()
        : base(Resource.Layout.SplashScreen)
    {
    }
}
