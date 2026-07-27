namespace Nivaes.App.Cross.WinUI
{
    [Obsolete("Heredar directamente de BaseWindowsPage")]
    public abstract class BaseView<TViewModel>
        : BaseWindowsPage<TViewModel>
        where TViewModel : IBaseViewModel
    {
    }
}
