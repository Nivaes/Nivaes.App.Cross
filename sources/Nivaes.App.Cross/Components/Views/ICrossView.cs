namespace Nivaes.App.Cross
{
    public interface ICrossView
    {
        ICrossViewModel? ViewModel { get; set; }
    }

    public interface ICrossView<TViewModel>
        where TViewModel : class, ICrossViewModel
    {
        TViewModel? ViewModel { get; set; }
    }
}
