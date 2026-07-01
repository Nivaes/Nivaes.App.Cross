namespace Nivaes.App.Cross.WinUI;

public interface ICrossWindowsViewModelLoader
{
    ICrossViewModel Load(string requestText, ICrossBundle savedState);
}
