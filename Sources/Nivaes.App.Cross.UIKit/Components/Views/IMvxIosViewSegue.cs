namespace Nivaes.App.Cross.UIKitLib
{
    using Foundation;

    public interface IMvxIosViewSegue
    {
        object PrepareViewModelParametersForSegue(UIStoryboardSegue segue, NSObject? sender);
    }
}
