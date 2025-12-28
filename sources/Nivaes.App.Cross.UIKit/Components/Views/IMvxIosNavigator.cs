namespace Nivaes.App.Cross.UIKitOS
{
    public interface IMvxIosNavigator
    {
        void NavigateTo(CrossViewModelRequest request);

        void Close(ICrossViewModel toClose);
    }
}
