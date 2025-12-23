namespace Nivaes.App.Cross.UIKit
{
    public interface IMvxIosNavigator
    {
        void NavigateTo(CrossViewModelRequest request);

        void Close(ICrossViewModel toClose);
    }
}
