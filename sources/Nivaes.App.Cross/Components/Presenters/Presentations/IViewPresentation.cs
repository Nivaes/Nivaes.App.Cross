namespace Nivaes.App.Cross.Presenters
{
    public interface IViewPresentation
    {
        Task<bool> ShowView(Type viewType, IViewModelRequest request);

        Task<bool> CloseView(IViewModel request);
    }
}
