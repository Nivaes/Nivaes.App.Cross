namespace Nivaes.App.Cross.WinUI3
{
    public interface ICrossWindowsContentDialog<TViewModel> :
        ICrossView<TViewModel> where TViewModel : class, ICrossViewModel
    {
    }
}
