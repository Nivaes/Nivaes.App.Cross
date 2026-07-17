namespace Nivaes.App.Cross.AppKitLib;

public interface IPressenterActionContext
{
    List<NSWindow> Windows { get; }

    NSWindow MainWindow { get;  }
}
