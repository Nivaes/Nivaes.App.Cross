using System.Diagnostics.CodeAnalysis;
using Android.Content.PM;
using Android.Runtime;
using Nivaes.App.Cross.Droid;

namespace Nivaes.App.Cross.Sample.Droid;

[Activity(Name = "com.nivaes.SplashScreen"
       , Label = "@string/app_name"
       , MainLauncher = true
       , Icon = "@drawable/ic_launcher"
       , Theme = "@style/AppTheme.Splash"
       , NoHistory = true)]
public sealed class SplashScreen 
    : StartActivity
{
    public SplashScreen()
        : base(Resource.Layout.SplashScreen)
    {
    }
}
