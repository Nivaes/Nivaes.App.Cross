namespace MvvmCross.Platforms.Ios.Views
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public interface IMvxIosNavigator
    {
        void NavigateTo(CrossViewModelRequest request);

        void Close(ICrossViewModel toClose);
    }
}
