namespace Nivaes.App.Cross.Droid
{
    public class MvxActivityEventArgs : EventArgs
    {
        public MvxActivityEventArgs(Activity activity, MvxActivityState state, object extras = null)
        {
            Activity = activity;
            ActivityState = state;
            Extras = extras;
        }

        public MvxActivityState ActivityState { get; }
        public Activity Activity { get; }
        public object Extras { get; }
    }
}
