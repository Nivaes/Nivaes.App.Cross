namespace MvvmCross.Platforms.Ios.Views
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public interface IMvxIosNavigator
    {
        void NavigateTo(MvxViewModelRequest request);

        void Close(ICrossViewModel toClose);
    }
}
