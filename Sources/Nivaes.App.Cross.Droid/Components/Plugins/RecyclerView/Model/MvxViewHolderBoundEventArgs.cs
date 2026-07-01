namespace Nivaes.App.Cross.Droid
{
    public class MvxViewHolderBoundEventArgs(
        int itemPosition,
        object? dataContext,
        AndroidX.RecyclerView.Widget.RecyclerView.ViewHolder holder)
        : EventArgs
    {
        public int ItemPosition { get; } = itemPosition;

        public object? DataContext { get; } = dataContext;

        public AndroidX.RecyclerView.Widget.RecyclerView.ViewHolder Holder { get; } = holder;
    }
}