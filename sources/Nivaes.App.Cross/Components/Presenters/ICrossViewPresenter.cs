namespace Nivaes.App.Cross
{
    public interface ICrossViewPresenter
    {
        Task<bool> Show(ICrossViewModelRequest request);

        Task<bool> Close(ICrossViewModel request);
    }
}
