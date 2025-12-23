namespace Nivaes.App.Cross.Droid
{
    using System;
    using Android.App;
    using Android.Content;
    using Microsoft.Extensions.Logging;
    using MvvmCross;
    using MvvmCross.Platforms.Android.Views.Base;
    using Nivaes.App.Cross;

    public class MvxAndroidTask
        : CrossMainThreadDispatchingObject
    {
        protected void StartActivity(Intent intent)
        {
            DoOnActivity(activity => activity.StartActivity(intent));
        }

        protected void StartActivityForResult(int requestCode, Intent intent)
        {
            DoOnActivity(activity =>
                {
                    var androidView = activity as IMvxStartActivityForResult;
                    if (androidView == null)
                    {
                        CrossLogHost.GetLog<MvxAndroidTask>()?.Log(LogLevel.Error, "Error - current activity is null or does not support IMvxAndroidView");
                        return;
                    }

                    Mvx.IoCProvider.Resolve<IMvxIntentResultSource>().Result += OnMvxIntentResultReceived;
                    androidView.MvxInternalStartActivityForResult(intent, requestCode);
                });
        }

        protected virtual void ProcessMvxIntentResult(MvxIntentResultEventArgs result)
        {
            // default processing does nothing
        }

        private void OnMvxIntentResultReceived(object sender, MvxIntentResultEventArgs e)
        {
            CrossLogHost.GetLog<MvxAndroidTask>()?.Log(LogLevel.Trace, "OnMvxIntentResultReceived in MvxAndroidTask");
            // TODO - is this correct - should we always remove the result registration even if this isn't necessarily our result?
            Mvx.IoCProvider.Resolve<IMvxIntentResultSource>().Result -= OnMvxIntentResultReceived;
            ProcessMvxIntentResult(e);
        }

        protected void DoOnActivity(Action<Activity> action, bool ensureOnMainThread = true)
        {
            var activity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

            if (ensureOnMainThread)
            {
                InvokeOnMainThread(() => action(activity));
            }
            else
            {
                action(activity);
            }
        }
    }
}
