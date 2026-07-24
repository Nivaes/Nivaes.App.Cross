namespace Nivaes.App.Cross.WinUI
{
    public abstract class BaseDetailView<TViewModel>
        : BaseView<TViewModel>
        where TViewModel : class, IBaseDetailViewModel
    {
    }
}
