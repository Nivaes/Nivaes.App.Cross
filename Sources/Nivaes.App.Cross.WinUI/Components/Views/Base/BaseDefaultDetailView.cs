namespace Nivaes.App.Cross.WinUI
{
    public abstract class BaseDefaultDetailView<TViewModel>
        : CrossWindowsPage<TViewModel>
        where TViewModel : BaseDefaultDetailViewModel, ICrossViewModel
    {
    }
}
