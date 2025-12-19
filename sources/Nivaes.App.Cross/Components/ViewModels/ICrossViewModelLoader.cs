namespace Nivaes.App.Cross
{
    public interface ICrossViewModelLoader
    {
        ICrossViewModel LoadViewModel(ICrossViewModelRequest request, ICrossBundle? savedState, ICrossNavigateEventArgs? navigationArgs = null);

        ICrossViewModel LoadViewModel<TParameter>(ICrossViewModelRequest request, TParameter param, ICrossBundle? savedState,
            ICrossNavigateEventArgs? navigationArgs = null);

        ICrossViewModel ReloadViewModel(ICrossViewModel viewModel, ICrossViewModelRequest request, ICrossBundle? savedState, ICrossNavigateEventArgs? navigationArgs = null);

        ICrossViewModel ReloadViewModel<TParameter>(ICrossViewModel<TParameter> viewModel, TParameter param,
            ICrossViewModelRequest request, ICrossBundle? savedState, ICrossNavigateEventArgs? navigationArgs = null);
    }
}