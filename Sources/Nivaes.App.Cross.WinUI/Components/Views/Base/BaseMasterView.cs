namespace Nivaes.App.Cross.WinUI
{
    public abstract class BaseMasterView<TViewModel>
        : BaseWindowsPage<TViewModel>
        where TViewModel : IMasterViewModel, ICrossViewModel
    {
    }
}
