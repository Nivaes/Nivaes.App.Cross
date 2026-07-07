namespace Nivaes.App.Cross.UIKitLib
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
        where TViewModel : ICrossViewModel
    {
        CrossFluentBindingDescriptionSet<IMvxIosView<TViewModel>, TViewModel> CreateBindingSet();
    }
}
