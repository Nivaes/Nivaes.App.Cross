namespace Nivaes.App.Cross.Droid
{
    using System;
    using Android.App;
    using Android.OS;
    using Object = Java.Lang.Object;

    public static class CrossJavaObjectExtensions
    {
        public static bool IsNull(this Object @object)
        {
            if (@object == null)
                return true;

            if (@object.Handle == IntPtr.Zero)
                return true;

            return false;
        }

        public static bool IsActivityDead(this Activity activity)
        {
            if (activity.IsNull())
                return true;

            if (activity.IsFinishing)
                return true;

            if (activity.IsDestroyed)
                return true;

            return false;
        }

        public static bool IsActivityAlive(this Activity activity) => !IsActivityDead(activity);
    }
}
