namespace Nivaes.App.Cross.WinUI3;

public interface IMvxWindowsContentDialog
    : ICrossView
{
}

public interface IMvxWindowsContentDialog<TViewModel>
    : IMvxWindowsContentDialog
    , ICrossView<TViewModel> where TViewModel : class, ICrossViewModel
{
}
