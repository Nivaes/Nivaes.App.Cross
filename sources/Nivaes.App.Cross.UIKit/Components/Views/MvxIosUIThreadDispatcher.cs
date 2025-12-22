namespace MvvmCross.Platforms.Ios.Views
{
    using System;
    using System.Threading;
    using MvvmCross.Base;
    using MvvmCross.Exceptions;
    using Nivaes.App.Cross;
    using UIKit;

    public abstract class MvxIosUIThreadDispatcher
        : CrossMainThreadAsyncDispatcher
    {
        private readonly SynchronizationContext _uiSynchronizationContext;

        protected MvxIosUIThreadDispatcher()
        {
            _uiSynchronizationContext = SynchronizationContext.Current;
            if (_uiSynchronizationContext == null)
                throw new CrossException("SynchronizationContext must not be null - check to make sure Dispatcher is created on UI thread");
        }

        public override bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            if (IsOnMainThread)
                ExceptionMaskedAction(action, maskExceptions);
            else
                UIApplication.SharedApplication.BeginInvokeOnMainThread(() =>
                {
                    ExceptionMaskedAction(action, maskExceptions);
                });
            return true;
        }

        public override bool IsOnMainThread => _uiSynchronizationContext == SynchronizationContext.Current;
    }
}
