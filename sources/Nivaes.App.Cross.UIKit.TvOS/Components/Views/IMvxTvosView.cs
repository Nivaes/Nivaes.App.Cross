namespace MvvmCross.Platforms.Tvos.Views
{
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public interface IMvxTvosView
        : ICrossView
        , IMvxCanCreateTvosView
        , IMvxBindingContextOwner
    {
        MvxViewModelRequest Request { get; set; }
    }

    public interface IMvxTvosView<TViewModel>
        : IMvxTvosView
        , ICrossView<TViewModel>
        where TViewModel : class, ICrossViewModel
    {
        MvxFluentBindingDescriptionSet<IMvxTvosView<TViewModel>, TViewModel> CreateBindingSet();
    }
}
