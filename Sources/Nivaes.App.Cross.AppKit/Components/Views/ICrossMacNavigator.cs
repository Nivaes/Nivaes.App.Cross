namespace Nivaes.App.Cross;

public interface ICrossMacNavigator
{
    void NavigateTo(CrossViewModelRequest request);

    void ChangePresentation(CrossPresentationHint hint);
}
