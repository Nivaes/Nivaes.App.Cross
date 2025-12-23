namespace Nivaes.App.Cross.Color
{
    public interface IMvxMacNavigator
    {
        void NavigateTo(CrossViewModelRequest request);

        void ChangePresentation(CrossPresentationHint hint);
    }
}
