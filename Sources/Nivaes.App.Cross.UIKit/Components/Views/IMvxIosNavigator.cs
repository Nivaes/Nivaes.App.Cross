namespace Nivaes.App.Cross.UIKitLib
{
    public interface IMvxIosNavigator
    {
        void NavigateTo(ViewModelRequest request);

        void Close(ICrossViewModel toClose);
    }
}
