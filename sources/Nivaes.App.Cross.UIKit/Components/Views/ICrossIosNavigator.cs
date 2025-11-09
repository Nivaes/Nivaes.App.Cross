namespace Nivaes.App.Cross.UIKit
{
    public interface ICrossIosNavigator
    {
        void NavigateTo(ICrossViewModelRequest request);

        void Close(ICrossViewModel toClose);
    }
}
