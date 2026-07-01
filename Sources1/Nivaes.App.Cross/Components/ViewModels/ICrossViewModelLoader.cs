namespace Nivaes.App.Cross
{
    public interface ICrossViewModelLoader
    {
        ICrossViewModel LoadViewModel(CrossViewModelRequest request, ICrossBundle? savedState, ICrossNavigateEventArgs? navigationArgs = null);

        ICrossViewModel LoadViewModel<TParameter>(CrossViewModelRequest request, TParameter param, ICrossBundle? savedState,
            ICrossNavigateEventArgs? navigationArgs = null);

        //ICrossViewModel LoadViewModel<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>(
        //    CrossViewModelRequest request, ICrossBundle? savedState, ICrossNavigateEventArgs? navigationArgs = null)
        //    where TViewModel : ICrossViewModel;

        //ICrossViewModel LoadViewModel<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel, TParameter>(
        //    CrossViewModelRequest request, TParameter param, ICrossBundle? savedState,
        //    ICrossNavigateEventArgs? navigationArgs = null)
        //    where TViewModel : ICrossViewModel;

        ICrossViewModel ReloadViewModel(ICrossViewModel viewModel, CrossViewModelRequest request, ICrossBundle? savedState, ICrossNavigateEventArgs? navigationArgs = null);

        ICrossViewModel ReloadViewModel<TParameter>(ICrossViewModel<TParameter> viewModel, TParameter param,
            CrossViewModelRequest request, ICrossBundle? savedState, ICrossNavigateEventArgs? navigationArgs = null);
    }
}