namespace Nivaes.App.Cross
{
    public interface ICrossView
        : ICrossDataConsumer
    {
        ICrossViewModel? ViewModel { get; set; }
    }

    public interface ICrossView<TViewModel> :
        ICrossView
        where TViewModel : ICrossViewModel
    {
        new TViewModel? ViewModel { get; set; }
    }
}
