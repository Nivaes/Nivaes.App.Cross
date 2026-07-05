namespace Nivaes.App.Cross.Droid.RecyclerView
{
    public interface IMvxRecyclerAdapterBindableHolder
    {
        event EventHandler<MvxViewHolderBoundEventArgs> MvxViewHolderBound;
    }
}