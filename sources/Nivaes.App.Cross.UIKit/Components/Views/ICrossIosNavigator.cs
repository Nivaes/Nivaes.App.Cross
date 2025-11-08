namespace Nivaes.App.Cross.UIKit
{
    public interface ICrossIosNavigator
    {
        void NavigateTo(CrossViewModelRequest request);

        void Close(ICrossViewModel toClose);
    }
}
