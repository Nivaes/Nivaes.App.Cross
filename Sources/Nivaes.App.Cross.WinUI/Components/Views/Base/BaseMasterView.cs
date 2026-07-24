namespace Nivaes.App.Cross.WinUI
{
    public abstract class BaseMasterView<TViewModel>
        : BaseView<TViewModel>
        where TViewModel : IMasterViewModel, ICrossViewModel
    {
    }
}
