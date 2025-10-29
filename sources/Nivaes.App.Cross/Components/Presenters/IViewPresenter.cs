namespace Nivaes.App.Cross.Presenters
{
    public interface IViewPresenter
    {
        Task<bool> Show(ICrossViewModelRequest request);

        Task<bool> Close(ICrossViewModel request);
    }
}
