namespace Nivaes.App.Cross
{
    using System;
    using System.Threading.Tasks;

    public interface ICrossViewDispatcher
    {
        Task<bool> ShowViewModel(ICrossViewModelRequest request);

        Task<bool> ShowViewModelOnMainThread(ICrossViewModelRequest request);

        Task<bool> ShowViewModelOnBackgroundThread(ICrossViewModelRequest request, Func<ICrossViewModelRequest, Task<bool>> action);

        Task<bool> ChangePresentation(CrossPresentationHint hint);
    }
}
