namespace Nivaes.App.Cross
{
    [Obsolete("Sustituir por ICrossView<TViewModel>")]
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
