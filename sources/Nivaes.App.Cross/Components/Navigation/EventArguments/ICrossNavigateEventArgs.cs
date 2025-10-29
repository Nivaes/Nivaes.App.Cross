namespace Nivaes.App.Cross
{
    public enum CrossNavigationMode
    {
        None,
        Show,
        Close
    }

    public interface ICrossNavigateEventArgs
    {
        bool Cancel { get; set; }
        CrossNavigationMode? Mode { get; set; }
        ICrossViewModel? ViewModel { get; set; }
    }
}
