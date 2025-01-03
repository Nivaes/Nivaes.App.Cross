namespace Nivaes.App.Cross
{
    using System.Threading.Tasks;

    public interface IViewDispatcher
    {
        Task<bool> ShowViewModel(IViewModelRequest request);

        Task<bool> ShowViewModelOnMainThread(IViewModelRequest request);

        Task<bool> ShowViewModelOnBackgroundThread(IViewModelRequest request, Func<IViewModelRequest, Task<bool>> action);
    }
}
