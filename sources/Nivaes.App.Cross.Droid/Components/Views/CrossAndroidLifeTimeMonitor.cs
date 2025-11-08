namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Threading;
    using Android.App;
    using Android.OS;

    // For lifetime explained, see http://developer.android.com/guide/topics/fundamentals/activities.html
    public class CrossAndroidLifeTimeMonitor
        : CrossLifetimeMonitor, ICrossAndroidActivityLifeTimeListener
    {
        private int _createdActivityCount;

        public virtual void OnCreate(Activity activity, Bundle eventArgs)
        {
            Interlocked.Increment(ref _createdActivityCount);

            if (_createdActivityCount == 1)
            {
                FireLifetimeChange(CrossLifetimeEvent.ActivatedFromDisk);
            }
            FireActivityChange(activity, CrossActivityState.OnCreate, eventArgs);
        }

        public virtual void OnStart(Activity activity)
        {
            FireActivityChange(activity, CrossActivityState.OnStart);
        }

        public virtual void OnRestart(Activity activity)
        {
            FireActivityChange(activity, CrossActivityState.OnRestart);
        }

        public virtual void OnResume(Activity activity)
        {
            FireActivityChange(activity, CrossActivityState.OnResume);
        }

        public virtual void OnPause(Activity activity)
        {
            FireActivityChange(activity, CrossActivityState.OnPause);
        }

        public virtual void OnStop(Activity activity)
        {
            FireActivityChange(activity, CrossActivityState.OnStop);
        }

        public virtual void OnDestroy(Activity activity)
        {
            Interlocked.Decrement(ref _createdActivityCount);

            if (_createdActivityCount == 0)
            {
                FireLifetimeChange(CrossLifetimeEvent.Closing);
            }
            FireActivityChange(activity, CrossActivityState.OnDestroy);
        }

        public virtual void OnViewNewIntent(Activity activity)
        {
            FireActivityChange(activity, CrossActivityState.OnNewIntent);
        }

        public virtual void OnSaveInstanceState(Activity activity, Bundle eventArgs)
        {
            FireActivityChange(activity, CrossActivityState.OnSaveInstanceState, eventArgs);
        }

        protected void FireActivityChange(Activity activity, CrossActivityState state, object extras = null)
        {
            ActivityChanged?.Invoke(this, new CrossActivityEventArgs(activity, state, extras));
        }

        public event EventHandler<CrossActivityEventArgs> ActivityChanged;
    }
}
