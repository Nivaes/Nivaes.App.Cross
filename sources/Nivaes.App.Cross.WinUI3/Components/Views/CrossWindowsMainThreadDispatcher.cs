using System;
using Microsoft.UI.Dispatching;

namespace Nivaes.App.Cross.WinUI3;

public class CrossWindowsMainThreadDispatcher 
    : CrossMainThreadAsyncDispatcher
{
    private readonly DispatcherQueue _uiDispatcher;

    public CrossWindowsMainThreadDispatcher(DispatcherQueue uiDispatcher)
    {
        _uiDispatcher = uiDispatcher;
    }

    public override bool IsOnMainThread => _uiDispatcher.HasThreadAccess;

    public override bool RequestMainThreadAction(Action action, bool maskExceptions = true)
    {
        if (IsOnMainThread)
        {
            ExceptionMaskedAction(action, maskExceptions);
            return true;
        }

        var queued = _uiDispatcher.TryEnqueue(DispatcherQueuePriority.Normal,
            () => ExceptionMaskedAction(action, maskExceptions));
        return queued;
    }
}
