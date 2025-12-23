namespace Nivaes.App.Cross.AppKit
{
    public interface IMvxMacView
        : ICrossView
            , IMvxCanCreateMacView
            , IMvxBindingContextOwner
    {
        CrossViewModelRequest? Request { get; set; }
    }

    public interface IMvxMacView<TViewModel>
        : IMvxMacView
        , ICrossView<TViewModel> where TViewModel : class, ICrossViewModel
    {
        MvxFluentBindingDescriptionSet<IMvxMacView<TViewModel>, TViewModel> CreateBindingSet();
    }
}
