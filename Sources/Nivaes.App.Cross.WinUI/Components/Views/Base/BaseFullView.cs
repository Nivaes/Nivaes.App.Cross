namespace Nivaes.App.Cross.WinUI
{
    public abstract class BaseFullView<TViewModel>
        : BaseView<TViewModel>
         where TViewModel : FullViewModel, ICrossViewModel
    {
    }
}
