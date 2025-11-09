namespace Nivaes.App.Cross
{
    public interface ICrossViewPresenter
    {
        Task<bool> Show(ICrossViewModelRequest request);

        Task<bool> ChangePresentation(CrossPresentationHint hint);

        void AddPresentationHintHandler<THint>(Func<THint, Task<bool>> action) where THint : CrossPresentationHint;

        Task<bool> Close(ICrossViewModel request);
    }
}
