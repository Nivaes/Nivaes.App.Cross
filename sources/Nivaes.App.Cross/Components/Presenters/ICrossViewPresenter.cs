namespace Nivaes.App.Cross.Presenters
{
    public interface ICrossViewPresenter
    {
        Task<bool> Show(ICrossViewModelRequest request);

        Task<bool> Close(ICrossViewModel request);
    }
}
