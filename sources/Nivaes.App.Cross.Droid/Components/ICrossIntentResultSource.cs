namespace Nivaes.App.Cross.Droid
{
    using System;

    public interface ICrossIntentResultSource
    {
        event EventHandler<CrossIntentResultEventArgs> Result;
    }
}
