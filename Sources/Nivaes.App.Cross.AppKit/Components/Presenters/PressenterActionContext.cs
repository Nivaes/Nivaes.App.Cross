namespace Nivaes.App.Cross.AppKitLib;

public class PressenterActionContext
    : IPressenterActionContext
{
    public List<NSWindow> Windows { get; } = new();

    public NSWindow MainWindow => NSApplication.SharedApplication.MainWindow;
}
