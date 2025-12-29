namespace Nivaes.App.Cross.WinUI3;

public interface ICrossWindowsContentDialog
    : ICrossView
{
}

public interface ICrossWindowsContentDialog<TViewModel>
    : ICrossWindowsContentDialog
    , ICrossView<TViewModel> where TViewModel : class, ICrossViewModel
{
}
