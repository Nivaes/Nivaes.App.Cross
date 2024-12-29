namespace Nivaes.App.Cross.Presenters
{
    public interface IViewPresenter
    {
        Task<bool> Show(IViewModelRequest request);

        Task<bool> Close(IViewModel request);
    }
}
