namespace Nivaes.App.Cross.WinUI
{
    public interface ICrossWindowsContentDialog<TViewModel> :
        ICrossView<TViewModel> where TViewModel : class, ICrossViewModel
    {
    }
}
