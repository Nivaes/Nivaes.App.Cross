namespace Nivaes.App.Cross.Droid
{
    using System;
    using Android.App;

    public class CrossActivityEventArgs : EventArgs
    {
        public CrossActivityEventArgs(Activity activity, CrossActivityState state, object extras = null)
        {
            Activity = activity;
            ActivityState = state;
            Extras = extras;
        }

        public CrossActivityState ActivityState { get; }
        public Activity Activity { get; }
        public object Extras { get; }
    }
}
