namespace Nivaes.App.Cross
{
    public enum NavigationMode
    {
        None,
        Show,
        Close
    }

    public interface INavigateEventArgs
    {
        bool Cancel { get; set; }
        NavigationMode? Mode { get; set; }
        IViewModel? ViewModel { get; set; }
    }
}
