namespace Nivaes.App.Cross
{
    using System.Threading.Tasks;

    public interface ICrossViewDispatcher : ICrossMainThreadAsyncDispatcher, ICrossMainThreadDispatcher
    {
        Task<bool> ShowViewModel(CrossViewModelRequest request);

        Task<bool> ChangePresentation(CrossPresentationHint hint);
    }
}
