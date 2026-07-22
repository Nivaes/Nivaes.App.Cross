namespace Nivaes.App.Cross.AppKitLib
{
    public interface IMvxMacView
        : ICrossView
            , IMvxCanCreateMacView
            , ICrossBindingContextOwner
    {
        IViewModelRequest? Request { get; set; }
    }

    public interface IMvxMacView<TViewModel>
        : IMvxMacView
        , ICrossView<TViewModel> where TViewModel : class, ICrossViewModel
    {
        CrossFluentBindingDescriptionSet<IMvxMacView<TViewModel>, TViewModel> CreateBindingSet();
    }
}
