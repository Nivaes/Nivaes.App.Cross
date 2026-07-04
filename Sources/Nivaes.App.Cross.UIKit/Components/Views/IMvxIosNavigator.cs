namespace Nivaes.App.Cross.UIKitLib
{
    public interface IMvxIosNavigator
    {
        void NavigateTo(CrossViewModelRequest request);

        void Close(ICrossViewModel toClose);
    }
}
