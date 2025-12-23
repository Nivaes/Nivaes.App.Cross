namespace Nivaes.App.Cross.WinUI3
{
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
