namespace Nivaes.App.Cross.UIKit.TvOS
{
    public interface IMvxTvosView
        : ICrossView
        , IMvxCanCreateTvosView
        , IMvxBindingContextOwner
    {
        CrossViewModelRequest Request { get; set; }
    }

    public interface IMvxTvosView<TViewModel>
        : IMvxTvosView
        , ICrossView<TViewModel>
        where TViewModel : class, ICrossViewModel
    {
        MvxFluentBindingDescriptionSet<IMvxTvosView<TViewModel>, TViewModel> CreateBindingSet();
    }
}
