namespace Nivaes.App.Cross
{
    using System;
    using System.Threading.Tasks;
    using MvvmCross.ViewModels;

    public interface ICrossViewPresenter
    {
        Task<bool> Show(CrossViewModelRequest request);

        Task<bool> ChangePresentation(CrossPresentationHint hint);

        void AddPresentationHintHandler<THint>(Func<THint, Task<bool>> action) where THint : CrossPresentationHint;

        Task<bool> Close(ICrossViewModel viewModel);
    }
}
