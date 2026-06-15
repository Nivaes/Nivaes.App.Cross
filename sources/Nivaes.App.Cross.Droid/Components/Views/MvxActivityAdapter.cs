using System.Diagnostics.CodeAnalysis;
using Android.Content;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.IoC;

namespace Nivaes.App.Cross.Droid;

[RequiresUnreferencedCode("Loading ViewModels requires unreferenced code")]
public class MvxActivityAdapter : MvxBaseActivityAdapter
{
    protected IMvxAndroidView? AndroidView => Activity as IMvxAndroidView;

    public MvxActivityAdapter(ICrossEventSourceActivity eventSource)
        : base(eventSource)
    {
    }

    protected override void EventSourceOnStopCalled(object? sender, EventArgs eventArgs)
    {
        AndroidView?.OnViewStop();
    }

    protected override void EventSourceOnStartCalled(object? sender, EventArgs eventArgs)
    {
        AndroidView?.OnViewStart();
    }

    protected override void EventSourceOnStartActivityForResultCalled(
        object? sender, CrossValueEventArgs<MvxStartActivityForResultParameters> eventArgs)
    {
        var requestCode = eventArgs.Value.RequestCode;
        switch (requestCode)
        {
            case (int)MvxIntentRequestCode.PickFromFile:
                var logger = IPlatformApplication.Current?.Services.GetRequiredService<ILogger<MvxActivityAdapter>>();
                logger?.Log(LogLevel.Warning,
                    "Warning - activity request code may clash with Mvx code for {requestCode}",
                    (MvxIntentRequestCode)requestCode);
                break;
        }
    }

    protected override void EventSourceOnResumeCalled(object? sender, EventArgs eventArgs)
    {
        AndroidView?.OnViewResume();
    }

    protected override void EventSourceOnRestartCalled(object? sender, EventArgs eventArgs)
    {
        AndroidView?.OnViewRestart();
    }

    protected override void EventSourceOnPauseCalled(object? sender, EventArgs eventArgs)
    {
        AndroidView?.OnViewPause();
    }

    protected override void EventSourceOnNewIntentCalled(object? sender, CrossValueEventArgs<Intent> eventArgs)
    {
        AndroidView?.OnViewNewIntent();
    }

    protected override void EventSourceOnDestroyCalled(object? sender, EventArgs eventArgs)
    {
        AndroidView?.OnViewDestroy();
    }

    protected override void EventSourceOnCreateCalled(object? sender, CrossValueEventArgs<Bundle> eventArgs)
    {
        AndroidView?.OnViewCreate(eventArgs.Value);
    }

    protected override void EventSourceOnSaveInstanceStateCalled(object? sender, CrossValueEventArgs<Bundle> eventArgs)
    {
        var mvxBundle = AndroidView?.CreateSaveStateBundle();
        if (mvxBundle != null)
        {
            if (Mvx.IoCProvider?.TryResolve<IMvxSavedStateConverter>(out var converter) != true)
            {
                var logger = IPlatformApplication.Current?.Services.GetRequiredService<ILogger<MvxActivityAdapter>>();
                logger?.Log(LogLevel.Warning,
                    "Saved state converter not available - saving state will be hard");
            }
            else
            {
                converter.Write(eventArgs.Value, mvxBundle);
            }
        }

        if (Mvx.IoCProvider?.TryResolve<IMvxSingleViewModelCache>(out var cache) == true)
        {
            cache.Cache(AndroidView.ViewModel, eventArgs.Value);
        }
    }

    protected override void EventSourceOnActivityResultCalled(
        object? sender, CrossValueEventArgs<MvxActivityResultParameters> eventArgs)
    {
        if (Mvx.IoCProvider?.TryResolve<IMvxIntentResultSink>(out var sink) == true)
        {
            var resultParameters = eventArgs.Value;
            var intentResult = new MvxIntentResultEventArgs(
                resultParameters.RequestCode,
                resultParameters.ResultCode,
                resultParameters.Data);
            sink.OnResult(intentResult);
        }
    }
}
