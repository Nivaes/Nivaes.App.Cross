namespace MvvmCross.Presenters
{
    using System;
    using System.Threading.Tasks;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public interface IMvxViewPresenter
    {
        Task<bool> Show(CrossViewModelRequest request);

        Task<bool> ChangePresentation(CrossPresentationHint hint);

        void AddPresentationHintHandler<THint>(Func<THint, Task<bool>> action) where THint : CrossPresentationHint;

        Task<bool> Close(ICrossViewModel viewModel);
    }
}
