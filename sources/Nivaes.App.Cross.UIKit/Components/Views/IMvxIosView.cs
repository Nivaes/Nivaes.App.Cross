namespace Nivaes.App.Cross.UIKit
{
    public interface IMvxIosView
        : ICrossView
        , IMvxCanCreateIosView
        , ICrossBindingContextOwner
    {
        CrossViewModelRequest? Request { get; set; }
    }

    public interface IMvxIosView<TViewModel>
        : IMvxIosView, ICrossView<TViewModel> 
        where TViewModel : class, ICrossViewModel
    {
        CrossFluentBindingDescriptionSet<IMvxIosView<TViewModel>, TViewModel> CreateBindingSet();
    }
}
