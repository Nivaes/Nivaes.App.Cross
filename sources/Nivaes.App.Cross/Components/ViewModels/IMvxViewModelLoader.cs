namespace MvvmCross.ViewModels
{
    using MvvmCross.Navigation.EventArguments;
    using Nivaes.App.Cross;

    public interface IMvxViewModelLoader
    {
        ICrossViewModel LoadViewModel(MvxViewModelRequest request, IMvxBundle? savedState, IMvxNavigateEventArgs? navigationArgs = null);

        ICrossViewModel LoadViewModel<TParameter>(MvxViewModelRequest request, TParameter param, IMvxBundle? savedState,
            IMvxNavigateEventArgs? navigationArgs = null);

        ICrossViewModel ReloadViewModel(ICrossViewModel viewModel, MvxViewModelRequest request, IMvxBundle? savedState, IMvxNavigateEventArgs? navigationArgs = null);

        ICrossViewModel ReloadViewModel<TParameter>(ICrossViewModel<TParameter> viewModel, TParameter param,
            MvxViewModelRequest request, IMvxBundle? savedState, IMvxNavigateEventArgs? navigationArgs = null);
    }
}