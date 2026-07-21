namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Threading;
    using Android.App;
    using Microsoft.Extensions.Logging;

    public class MvxAndroidMainThreadDispatcher
        : CrossMainThreadDispatcher
    {
        public override bool IsOnMainThread => Application.SynchronizationContext == SynchronizationContext.Current;

        public MvxAndroidMainThreadDispatcher(ILogger<MvxAndroidMainThreadDispatcher> logger)
            : base(logger)
        {
        }

        [Obsolete("", true)]
        public override bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            if (IsOnMainThread)
            {
                ExceptionMaskedAction(action, maskExceptions);
            }
            else
            {
                Application.SynchronizationContext.Post(ignored =>
                {
                    ExceptionMaskedAction(action, maskExceptions);
                }, null);
            }

            return true;
        }
    }
}
