namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Threading;
    using Android.App;

    public class MvxAndroidMainThreadDispatcher 
        : CrossMainThreadAsyncDispatcher
    {
        public override bool IsOnMainThread => Application.SynchronizationContext == SynchronizationContext.Current;

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
