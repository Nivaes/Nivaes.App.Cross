namespace Nivaes.App.Cross
{
    public interface ICrossViewDispatcher 
        : ICrossMainThreadDispatcher
    {
        Task<bool> ShowViewModel(ViewModelRequest request);

        Task<bool> ChangePresentation(CrossPresentationHint hint);
    }
}
