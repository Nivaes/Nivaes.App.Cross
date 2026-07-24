namespace Nivaes.App.Cross.WinUI
{
    public abstract class BasePagerView<TViewModel>
        : BaseView<TViewModel>
        where TViewModel : BasePagerViewModel, ICrossViewModel
    {
    }
}
