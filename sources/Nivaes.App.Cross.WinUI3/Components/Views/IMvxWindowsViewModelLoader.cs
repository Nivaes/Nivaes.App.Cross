namespace Nivaes.App.Cross.WinUI3;

public interface IMvxWindowsViewModelLoader
{
    ICrossViewModel Load(string requestText, ICrossBundle savedState);
}
