namespace Nivaes.App.Cross.AppKit
{
    using Foundation;

    public interface IMvxMacViewSegue
    {
        object PrepareViewModelParametersForSegue(NSStoryboardSegue segue, NSObject sender);
    }
}
