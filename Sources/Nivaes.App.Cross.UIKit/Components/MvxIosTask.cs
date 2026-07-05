namespace Nivaes.App.Cross.UIKitLib;

public class MvxIosTask
{
    protected Task<bool> DoUrlOpen(NSUrl url)
    {
        var sharedApp = UIApplication.SharedApplication;
        var options = new UIApplicationOpenUrlOptions { UniversalLinksOnly = false };
        var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        if (sharedApp.CanOpenUrl(url))
        {
            sharedApp.OpenUrl(url, options, ok => tcs.TrySetResult(ok));
        }
        else
        {
            tcs.TrySetResult(false);
        }

        return tcs.Task;
    }
}
