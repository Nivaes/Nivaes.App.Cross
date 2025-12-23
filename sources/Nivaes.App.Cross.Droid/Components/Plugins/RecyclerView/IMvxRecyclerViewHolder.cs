namespace Nivaes.App.Cross.Droid
{
    public interface IMvxRecyclerViewHolder : IMvxBindingContextOwner
    {
        event EventHandler<EventArgs>? Click;
        event EventHandler<EventArgs>? LongClick;

        int Id { get; set; }
        object? DataContext { get; set; }

        void OnAttachedToWindow();
        void OnDetachedFromWindow();
        void OnViewRecycled();
    }
}