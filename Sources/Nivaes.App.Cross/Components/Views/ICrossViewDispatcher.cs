namespace Nivaes.App.Cross
{
    public interface ICrossViewDispatcher 
        : ICrossMainThreadDispatcher
    {
        Task<bool> ShowViewModel(IViewModelRequest request);

        Task<bool> ChangePresentation(CrossPresentationHint hint);
    }
}
