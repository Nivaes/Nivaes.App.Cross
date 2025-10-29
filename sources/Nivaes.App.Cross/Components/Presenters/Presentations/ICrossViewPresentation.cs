namespace Nivaes.App.Cross.Presenters
{
    public interface ICrossViewPresentation
    {
        Task<bool> ShowView(Type viewType, ICrossViewModelRequest request);

        Task<bool> CloseView(ICrossViewModel request);
    }
}
