namespace Nivaes.App.Cross.WinUI
{
    public abstract class BaseView<TViewModel>
        : BaseWindowsPage<TViewModel>
        where TViewModel : IBaseViewModel
    {
    }
}
