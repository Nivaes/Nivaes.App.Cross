namespace Nivaes.App.Cross.Droid
{
    using System;

    public class MvxIntentResultSink
        : IMvxIntentResultSink, IMvxIntentResultSource
    {
        public void OnResult(MvxIntentResultEventArgs result)
        {
            Result?.Invoke(this, result);
        }

        public event EventHandler<MvxIntentResultEventArgs>? Result;
    }
}
