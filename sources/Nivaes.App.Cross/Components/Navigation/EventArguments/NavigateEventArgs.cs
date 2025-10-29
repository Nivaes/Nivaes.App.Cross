namespace Nivaes.App.Cross
{
    public interface IMvxNavigateEventArgs
    {
        bool Cancel { get; set; }
        NavigationMode Mode { get; set; }
        IViewModel ViewModel { get; set; }
    }
}
