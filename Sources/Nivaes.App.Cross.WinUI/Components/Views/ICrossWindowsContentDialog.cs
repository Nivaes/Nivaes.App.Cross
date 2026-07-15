namespace Nivaes.App.Cross.WinUI;

public interface ICrossWindowsContentDialog
    : ICrossView
{
}

public interface ICrossWindowsContentDialog<TViewModel>
    : ICrossWindowsContentDialog
    , ICrossView<TViewModel> 
    where TViewModel : class, ICrossViewModel
{
}
