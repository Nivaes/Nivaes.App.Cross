namespace Nivaes.App.Cross.Droid
{
    using System;

    [Obsolete()]
    public interface ICrossIntentResultSource
    {
        event EventHandler<CrossIntentResultEventArgs> Result;
    }
}
