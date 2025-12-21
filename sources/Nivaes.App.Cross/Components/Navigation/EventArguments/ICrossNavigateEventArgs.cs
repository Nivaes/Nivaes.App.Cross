namespace Nivaes.App.Cross
{
    public interface ICrossNavigateEventArgs
    {
        bool Cancel { get; set; }
        NavigationMode Mode { get; set; }
        ICrossViewModel? ViewModel { get; set; }
    }
}