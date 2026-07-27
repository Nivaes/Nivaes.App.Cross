namespace Nivaes.App.Cross.WinUI
{
    public abstract class BaseDetailView<TViewModel>
        : CrossWindowsPage<TViewModel>
        where TViewModel : class, IBaseDetailViewModel
    {
    }
}
