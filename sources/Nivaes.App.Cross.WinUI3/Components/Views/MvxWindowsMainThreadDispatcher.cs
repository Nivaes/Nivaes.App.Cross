namespace MvvmCross.Platforms.WinUi.Views
{
    using System;
    using Microsoft.UI.Dispatching;
    using MvvmCross.Base;
    using Nivaes.App.Cross;
    using Windows.UI.Core;

    public class MvxWindowsMainThreadDispatcher 
        : CrossMainThreadAsyncDispatcher
    {
        private readonly DispatcherQueue _uiDispatcher;

        public MvxWindowsMainThreadDispatcher(DispatcherQueue uiDispatcher)
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
}
