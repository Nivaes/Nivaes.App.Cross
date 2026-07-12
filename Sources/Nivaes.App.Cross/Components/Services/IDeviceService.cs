namespace Nivaes.App.Cross;

public interface IDeviceService
{
    byte[] GetUniqueIdentifier();

    string GetVersionApp();

    void RestartApp();
}
