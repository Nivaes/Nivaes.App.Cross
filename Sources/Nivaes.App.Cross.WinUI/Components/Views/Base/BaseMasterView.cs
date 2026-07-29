namespace Nivaes.App.Cross.WinUI
{
    public abstract class BaseMasterView<TViewModel>
        : CrossWindowsPage<TViewModel>
        where TViewModel : IMasterViewModel, ICrossViewModel
    {
    }
}
