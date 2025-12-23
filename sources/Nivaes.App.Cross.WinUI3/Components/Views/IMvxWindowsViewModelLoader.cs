namespace Nivaes.App.Cross.WinUI3
{
    using Nivaes.App.Cross;

    public interface IMvxWindowsViewModelLoader
    {
        ICrossViewModel Load(string requestText, ICrossBundle savedState);
    }
}
