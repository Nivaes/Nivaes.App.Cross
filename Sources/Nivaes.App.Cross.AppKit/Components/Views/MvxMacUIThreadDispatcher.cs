using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.AppKitLib
{
    public abstract class MvxMacUIThreadDispatcher
        : CrossMainThreadAsyncDispatcher
    {
        private readonly SynchronizationContext _uiSynchronizationContext;

        protected MvxMacUIThreadDispatcher(ILogger logger)
            :base(logger)
        {
            _uiSynchronizationContext = SynchronizationContext.Current!;
            if (_uiSynchronizationContext == null)
                throw new CrossException("SynchronizationContext must not be null - check to make sure Dispatcher is created on UI thread");
        }

        public override bool RequestMainThreadAction(Action action,
            bool maskExceptions = true)
        {
            if (IsOnMainThread)
                ExceptionMaskedAction(action, maskExceptions);
            else
                NSApplication.SharedApplication.BeginInvokeOnMainThread(() =>
                {
                    ExceptionMaskedAction(action, maskExceptions);
                });
            return true;
        }

        public override bool IsOnMainThread => _uiSynchronizationContext == SynchronizationContext.Current;
    }
}
