namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Android.App;
    using Android.Content;
    using Android.OS;
    using Android.Runtime;
    using Android.Views;
    using Android.Widget;
    using MvvmCross.Platforms.Android.Views;

    [Android.Runtime.Preserve(AllMembers = true)]
    public static class LinkerPleaseInclude
    {

        public static void Include(SplashScreenActivity activity)
        {
            activity = new SplashScreenActivity();

            activity.Dispose();
        }

        //public static void Include(MvxAndroidApplication _)
        //{

        //}
    }
}
