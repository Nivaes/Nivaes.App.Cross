namespace MvvmCross.Platforms.Mac.Views
{
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public interface IMvxMacView
        : ICrossView
            , IMvxCanCreateMacView
            , IMvxBindingContextOwner
    {
        MvxViewModelRequest Request { get; set; }
    }

    public interface IMvxMacView<TViewModel>
        : IMvxMacView
        , ICrossView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        MvxFluentBindingDescriptionSet<IMvxMacView<TViewModel>, TViewModel> CreateBindingSet();
    }
}
