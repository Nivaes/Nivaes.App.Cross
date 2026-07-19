//namespace Nivaes.App.Cross
//{
//    internal interface ICrossViewModelLoader
//    {
//        ICrossViewModel? LoadViewModel(ViewModelRequest request, ICrossBundle? savedState, ICrossNavigateEventArgs? navigationArgs = null);

//        ICrossViewModel? LoadViewModel<TParameter>(ViewModelRequest request, TParameter param, ICrossBundle? savedState,
//            ICrossNavigateEventArgs? navigationArgs = null);

//        ICrossViewModel ReloadViewModel(ICrossViewModel viewModel, ViewModelRequest request, ICrossBundle? savedState, ICrossNavigateEventArgs? navigationArgs = null);

//        ICrossViewModel ReloadViewModel<TParameter>(ICrossViewModel<TParameter> viewModel, TParameter param,
//            ViewModelRequest request, ICrossBundle? savedState, ICrossNavigateEventArgs? navigationArgs = null);
//    }
//}