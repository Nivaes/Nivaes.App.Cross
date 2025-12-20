namespace Nivaes.App.Cross
{
    using System.Threading.Tasks;
    using MvvmCross.Base;
    using MvvmCross.ViewModels;

    public interface ICrossViewDispatcher : IMvxMainThreadAsyncDispatcher, IMvxMainThreadDispatcher
    {
        Task<bool> ShowViewModel(MvxViewModelRequest request);

        Task<bool> ChangePresentation(MvxPresentationHint hint);
    }
}
