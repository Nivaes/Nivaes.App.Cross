namespace Nivaes.App.Cross.WinUI
{
    public abstract class BaseDetailView<TViewModel>
        : BaseWindowsPage<TViewModel>
        where TViewModel : class, IBaseDetailViewModel
    {
    }
}
