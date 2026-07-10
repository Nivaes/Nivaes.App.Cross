namespace Nivaes.App.Cross
{
    public interface ICrossBindingContextOwner
    {
        ICrossBindingContext BindingContext { get; set; }
    }
}
