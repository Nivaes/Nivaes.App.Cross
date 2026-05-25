namespace Nivaes.App.Cross.WinUI3;

public interface ICrossWindowsViewModelLoader
{
    ICrossViewModel Load(string requestText, ICrossBundle savedState);
}
