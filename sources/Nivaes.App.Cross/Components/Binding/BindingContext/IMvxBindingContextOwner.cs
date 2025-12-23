namespace Nivaes.App.Cross
{
    public interface IMvxBindingContextOwner
    {
        IMvxBindingContext? BindingContext { get; set; }
    }
}
