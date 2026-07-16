namespace Nivaes.App.Cross
{
    // ToDo Unificar ICrossMainThreadAsyncDispatcher y ICrossMainThreadDispatcher.
    public interface ICrossViewDispatcher 
        : ICrossMainThreadAsyncDispatcher, ICrossMainThreadDispatcher
    {
        Task<bool> ShowViewModel(CrossViewModelRequest request);

        Task<bool> ChangePresentation(CrossPresentationHint hint);
    }
}
