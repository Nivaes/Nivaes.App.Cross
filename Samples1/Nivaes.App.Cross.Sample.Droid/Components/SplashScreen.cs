using System.Diagnostics.CodeAnalysis;
using Android.Content.PM;
using Nivaes.App.Cross.Droid;

namespace Nivaes.App.Cross.Sample.Droid;

[Activity(
    Label = "Nivaes.App.Droid"
    , MainLauncher = true
    , Icon = "@mipmap/icon"
    , Theme = "@style/AppTheme.Splash"
    , NoHistory = true
    , ScreenOrientation = ScreenOrientation.Portrait)]
[RequiresUnreferencedCode("CrossStartActivity require unreferenced code")]
public class SplashScreen : CrossStartActivity
{
    public SplashScreen()
        : base(Resource.Layout.SplashScreen)
    {
    }
}
