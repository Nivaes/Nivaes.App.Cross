namespace Nivaes.App.Cross.WinUI
{
    public abstract class BasePagerView<TViewModel>
        : BaseWindowsPage<TViewModel>
        where TViewModel : BasePagerViewModel, ICrossViewModel
    {
    }
}
