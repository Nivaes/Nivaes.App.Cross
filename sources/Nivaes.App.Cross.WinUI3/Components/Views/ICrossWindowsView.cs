namespace Nivaes.App.Cross.WinUI
{
    [Obsolete]
    public interface ICrossWindowsView
        : ICrossView
    {
        void ClearBackStack();
    }

    public interface ICrossWindowsView<TViewModel>
        : ICrossWindowsView,
        ICrossView<TViewModel> 
        where TViewModel : class, ICrossViewModel
    {
    }
}
