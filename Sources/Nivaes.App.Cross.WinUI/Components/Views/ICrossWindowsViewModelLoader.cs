namespace Nivaes.App.Cross.WinUI;

public interface ICrossWindowsViewModelLoader
{
    ICrossViewModel Load(byte[] requestBuffer, ICrossBundle savedState);
}
