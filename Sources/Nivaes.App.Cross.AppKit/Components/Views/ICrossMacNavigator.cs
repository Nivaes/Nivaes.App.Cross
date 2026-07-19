namespace Nivaes.App.Cross;

public interface ICrossMacNavigator
{
    void NavigateTo(ViewModelRequest request);

    void ChangePresentation(CrossPresentationHint hint);
}
