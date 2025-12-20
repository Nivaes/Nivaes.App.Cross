namespace MvvmCross.Platforms.Ios.Views
{
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public interface IMvxIosView
        : ICrossView
        , IMvxCanCreateIosView
        , IMvxBindingContextOwner
    {
        MvxViewModelRequest Request { get; set; }
    }

    public interface IMvxIosView<TViewModel>
        : IMvxIosView
        , ICrossView<TViewModel> where TViewModel : class, ICrossViewModel
    {
        MvxFluentBindingDescriptionSet<IMvxIosView<TViewModel>, TViewModel> CreateBindingSet();
    }
}
