using System.Diagnostics;

namespace Nivaes.App.Cross;

public interface IPlatformApplication
{
    public static IPlatformApplication? Current { [DebuggerHidden] get; [DebuggerHidden] set; }

    public IServiceProvider ServiceProvider { [DebuggerHidden] get; }

    public ICrossApplication Application { [DebuggerHidden] get; }
}
