namespace Nivaes.App.Cross.UIKit
{
    using Foundation;

    public interface IMvxIosViewSegue
    {
        object PrepareViewModelParametersForSegue(UIStoryboardSegue segue, NSObject? sender);
    }
}
