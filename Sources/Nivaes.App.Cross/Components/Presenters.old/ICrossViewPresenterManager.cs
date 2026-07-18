namespace Nivaes.App.Cross
{
    public interface ICrossViewPresenterManager
    {
        ValueTask<bool> Show(CrossViewModelRequest request);

        ValueTask<bool> Close(ICrossViewModel viewModel);

        ValueTask<bool> ChangePresentation(CrossPresentationHint hint);

        void AddPresentationHintHandler<THint>(Func<THint, ValueTask<bool>> action)
            where THint : CrossPresentationHint;
    }
}
