namespace Nivaes.App.Cross.Droid
{
    using Android.Runtime;

    [Register("mvvmcross.platforms.android.MvxCurrentTopActivity")]
    public class CrossCurrentTopActivity
        : Java.Lang.Object, Application.IActivityLifecycleCallbacks, ICrossAndroidCurrentTopActivity
    {
        private readonly WeakReference<Activity?> _lastSeenActivity = new(null);

        public Activity? Activity
        {
            get
            {
                if (_lastSeenActivity?.TryGetTarget(out var activity) ?? false)
                    return activity;
                return null;
            }
        }

        public static bool Initialized { get; set; }

        public void OnActivityCreated(Activity activity, Bundle? savedInstanceState)
        {
            _lastSeenActivity.SetTarget(activity);
        }

        public void OnActivityPaused(Activity activity)
        {
            _lastSeenActivity.SetTarget(activity);
        }

        public void OnActivityResumed(Activity activity)
        {
            _lastSeenActivity.SetTarget(activity);
        }

        public void OnActivityDestroyed(Activity activity)
        {
            if (Activity == activity)
                _lastSeenActivity.SetTarget(default);
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
            // not interested in this
        }

        public void OnActivityStarted(Activity activity)
        {
            // not interested in this
        }

        public void OnActivityStopped(Activity activity)
        {
            // not interested in this
        }
    }
}