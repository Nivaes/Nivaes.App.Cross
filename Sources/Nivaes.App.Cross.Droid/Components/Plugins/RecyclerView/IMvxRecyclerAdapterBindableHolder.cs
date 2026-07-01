namespace Nivaes.App.Cross.Droid
{
    using System;

    public interface IMvxRecyclerAdapterBindableHolder
    {
        event EventHandler<MvxViewHolderBoundEventArgs> MvxViewHolderBound;
    }
}