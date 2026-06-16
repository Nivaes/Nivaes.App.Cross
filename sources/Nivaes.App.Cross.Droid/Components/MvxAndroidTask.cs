using Android.Content;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross;
using Nivaes.IoC;

namespace Nivaes.App.Cross.Droid;


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
                    var logger = IPlatformApplication.Current?.Services.GetRequiredService<ILogger<MvxAndroidTask>>();
                    logger?.Log(LogLevel.Error, "Error - current activity is null or does not support IMvxAndroidView");
                    return;
                }

                IPlatformApplication.Current!.Services.GetRequiredService<IMvxIntentResultSource>().Result += OnMvxIntentResultReceived;
                androidView.MvxInternalStartActivityForResult(intent, requestCode);
            });
    }

    protected virtual void ProcessMvxIntentResult(MvxIntentResultEventArgs result)
    {
        // default processing does nothing
    }

    private void OnMvxIntentResultReceived(object sender, MvxIntentResultEventArgs e)
    {
        var logger = IPlatformApplication.Current?.Services.GetRequiredService<ILogger<MvxAndroidTask>>();
        logger?.Log(LogLevel.Trace, "OnMvxIntentResultReceived in MvxAndroidTask");

        // TODO - is this correct - should we always remove the result registration even if this isn't necessarily our result?
        IPlatformApplication.Current!.Services.GetRequiredService<IMvxIntentResultSource>().Result -= OnMvxIntentResultReceived;
        ProcessMvxIntentResult(e);
    }

    protected void DoOnActivity(Action<Activity> action, bool ensureOnMainThread = true)
    {
        var activity = IPlatformApplication.Current!.Services.GetRequiredService<IMvxAndroidCurrentTopActivity>().Activity;

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
