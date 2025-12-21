namespace MvvmCross.Platforms.Mac.Views
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public interface IMvxMacNavigator
    {
        void NavigateTo(CrossViewModelRequest request);

        void ChangePresentation(CrossPresentationHint hint);
    }
}
