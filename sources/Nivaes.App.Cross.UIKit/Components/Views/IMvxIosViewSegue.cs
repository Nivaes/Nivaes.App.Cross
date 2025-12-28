namespace Nivaes.App.Cross.UIKitOS
{
    using Foundation;

    public interface IMvxIosViewSegue
    {
        object PrepareViewModelParametersForSegue(UIStoryboardSegue segue, NSObject? sender);
    }
}
