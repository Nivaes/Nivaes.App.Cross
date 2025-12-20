namespace MvvmCross.Platforms.WinUi.Views
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public interface IMvxWindowsView
        : ICrossView
    {
        void ClearBackStack();
    }

    public interface IMvxWindowsView<TViewModel>
        : IMvxWindowsView
        , ICrossView<TViewModel> where TViewModel : class, ICrossViewModel
    {
    }
}
