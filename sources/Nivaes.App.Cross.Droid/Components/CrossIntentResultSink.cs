namespace Nivaes.App.Cross.Droid
{
    using System;

    public class CrossIntentResultSink : ICrossIntentResultSink, ICrossIntentResultSource
    {
        public void OnResult(CrossIntentResultEventArgs result)
        {
            Result?.Invoke(this, result);
        }

        public event EventHandler<CrossIntentResultEventArgs> Result;
    }
}
