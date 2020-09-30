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

    [Android.Runtime.Preserve(AllMembers = true)]
    public class LinkerPleaseInclude
    {
#pragma warning disable CA1822 // Mark members as static
        public void Include(SplashScreenActivity activity)
        {
            activity = new SplashScreenActivity();

            activity.Dispose();
        }
#pragma warning restore CA1822 // Mark members as static
    }
}
