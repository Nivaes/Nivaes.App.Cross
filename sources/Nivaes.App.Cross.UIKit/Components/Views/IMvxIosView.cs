namespace Nivaes.App.Cross.UIKit
{
    public interface IMvxIosView
        : ICrossView
        , IMvxCanCreateIosView
        , IMvxBindingContextOwner
    {
        CrossViewModelRequest? Request { get; set; }
    }

    public interface IMvxIosView<TViewModel>
        : IMvxIosView, ICrossView<TViewModel> 
        where TViewModel : class, ICrossViewModel
    {
        MvxFluentBindingDescriptionSet<IMvxIosView<TViewModel>, TViewModel> CreateBindingSet();
    }
}
