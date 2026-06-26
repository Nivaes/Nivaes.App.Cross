using System.Diagnostics;

namespace Nivaes.App.Cross;

public interface IPlatformApplication
{
    public static IPlatformApplication? Current { [DebuggerHidden] get; [DebuggerHidden] set; }

    public IServiceProvider Services { [DebuggerHidden] get; }

    public IApplication Application { [DebuggerHidden] get; }
}
