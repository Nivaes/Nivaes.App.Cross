namespace Nivaes.App.Cross.UIKit
{
    public interface ICrossIosViewSegue
    {
        object PrepareViewModelParametersForSegue(UIStoryboardSegue segue, NSObject sender);
    }
}
