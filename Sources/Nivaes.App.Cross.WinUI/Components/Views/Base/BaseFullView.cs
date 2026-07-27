namespace Nivaes.App.Cross.WinUI
{
    public abstract class BaseFullView<TViewModel>
        : BaseWindowsPage<TViewModel>
         where TViewModel : FullViewModel, ICrossViewModel
    {
    }
}
