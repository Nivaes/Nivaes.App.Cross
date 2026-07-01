namespace Nivaes.App.Cross.Droid
{
    using System;

    public interface IMvxIntentResultSource
    {
        event EventHandler<MvxIntentResultEventArgs> Result;
    }
}
