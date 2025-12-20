namespace MvvmCross.Presenters
{
    using System;
    using System.Threading.Tasks;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public interface IMvxViewPresenter
    {
        Task<bool> Show(MvxViewModelRequest request);

        Task<bool> ChangePresentation(MvxPresentationHint hint);

        void AddPresentationHintHandler<THint>(Func<THint, Task<bool>> action) where THint : MvxPresentationHint;

        Task<bool> Close(ICrossViewModel viewModel);
    }
}
