namespace Nivaes.App.Cross.Presenters
{
    public interface IViewPresentation
    {
        Task<bool> ShowView(Type viewType, ICrossViewModelRequest request);

        Task<bool> CloseView(ICrossViewModel request);
    }
}
