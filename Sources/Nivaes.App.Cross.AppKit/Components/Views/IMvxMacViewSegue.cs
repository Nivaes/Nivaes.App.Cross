namespace Nivaes.App.Cross.AppKitLib
{
    using Foundation;

    public interface IMvxMacViewSegue
    {
        object PrepareViewModelParametersForSegue(NSStoryboardSegue segue, NSObject sender);
    }
}
