namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Collections.Specialized;

    public interface IMvxAdapterWithChangedEvent
        : IMvxAdapter
    {
        event EventHandler<NotifyCollectionChangedEventArgs> DataSetChanged;
    }
}
