namespace Nivaes.App.Cross
{
    using MvvmCross.Navigation.EventArguments;
    using Nivaes.App.Cross;

    public interface ICrossViewModelLoader
    {
        ICrossViewModel LoadViewModel(CrossViewModelRequest request, ICrossBundle? savedState, IMvxNavigateEventArgs? navigationArgs = null);

        ICrossViewModel LoadViewModel<TParameter>(CrossViewModelRequest request, TParameter param, ICrossBundle? savedState,
            IMvxNavigateEventArgs? navigationArgs = null);

        ICrossViewModel ReloadViewModel(ICrossViewModel viewModel, CrossViewModelRequest request, ICrossBundle? savedState, IMvxNavigateEventArgs? navigationArgs = null);

        ICrossViewModel ReloadViewModel<TParameter>(ICrossViewModel<TParameter> viewModel, TParameter param,
            CrossViewModelRequest request, ICrossBundle? savedState, IMvxNavigateEventArgs? navigationArgs = null);
    }
}