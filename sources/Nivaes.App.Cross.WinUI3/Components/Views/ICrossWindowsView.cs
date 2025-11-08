namespace Nivaes.App.Cross.WinUI3
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
