using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.AppKitLib;

public class MacViewPresenterManager
    : CrossAttributeViewPresenterManager, IMacViewPresenterManager, ICrossAttributeViewPresenterManager
{
    private readonly INSApplicationDelegate _applicationDelegate;

    /// <summary>
    /// NSWindow keeps only the *weak* reference to its NSWindowController. So, the controller will be
    /// prematurely disposed if no other references exist. This table keeps a strong reference to the
    /// controller keeping it alive while the associated NSWindow is alive. Ref. issue #2198
    /// </summary>
    protected readonly ConditionalWeakTable<NSWindow, NSWindowController> _windowsToWindowControllers = new();

    public override BasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
    {
        Logger.Log(LogLevel.Trace, $"PresentationAttribute not found for {viewType.Name}. Assuming new window presentation", viewType.Name);
        return new WindowPresentationAttribute { ViewModelType = viewModelType, ViewType = viewType };
    }

    protected virtual INSApplicationDelegate ApplicationDelegate => _applicationDelegate;

    protected virtual List<NSWindow> Windows { get; } = new List<NSWindow>();

    protected virtual NSWindow MainWindow => NSApplication.SharedApplication.MainWindow;

    public MacViewPresenterManager(INSApplicationDelegate applicationDelegate, ICrossViewsContainer crossViewsContainer,
        ILogger<MacViewPresenterManager> logger)
        : base(crossViewsContainer, logger)
    {
        _applicationDelegate = applicationDelegate;
        NSWindow.Notifications.ObserveWillClose(OnWindowWillCloseNotification);
    }

    protected void OnWindowWillCloseNotification(object? sender, NSNotificationEventArgs e)
    {
        var window = e.Notification.Object as NSWindow;
        if (Windows.Contains(window))
            Windows.Remove(window);
    }
}
