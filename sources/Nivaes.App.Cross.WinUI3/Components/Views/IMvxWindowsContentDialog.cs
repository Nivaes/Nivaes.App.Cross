namespace MvvmCross.Platforms.WinUi.Views
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public interface IMvxWindowsContentDialog
        : ICrossView
    {
    }

    public interface IMvxWindowsContentDialog<TViewModel>
        : IMvxWindowsContentDialog
        , ICrossView<TViewModel> where TViewModel : class, ICrossViewModel
    {
    }
}
