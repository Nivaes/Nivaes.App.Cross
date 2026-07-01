namespace Nivaes.App.Cross.AppKitOS
{
    using Foundation;

    public interface IMvxMacViewSegue
    {
        object PrepareViewModelParametersForSegue(NSStoryboardSegue segue, NSObject sender);
    }
}
