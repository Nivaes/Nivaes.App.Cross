using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.Graphics;
using Windows.UI.Core;
using Control = Microsoft.UI.Xaml.Controls.Control;
using HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment;
using Window = Microsoft.UI.Xaml.Window;

namespace Nivaes.App.Cross.WinUI;

/// <summary>
///     Defines a view presenter with multi-windows support.
/// </summary>
public class MultiWindowViewPresenterManager
    : CrossViewPresenterManager, IWindowsViewPresenterManager //, IMvxMultiWindowsService
{
    private const int DefaultWindowHeight = 456;
    private const int DefaultWindowWidth = 786;

    private const string WindowTitle = "WindowTitle";
    private readonly IServiceProvider _serviceProvider;
    private readonly WindowInformation _mainFrame;
    private readonly List<WindowInformation> _windowInformation = new();
    private readonly ICrossWindowsViewModelRequestTranslator _requestTranslator;

    private readonly Lock _windowInformationLock = new();

    private readonly ICrossViewModelLoader _viewModelLoader;

    /// <summary>
    ///     Initializes a new instance of <see cref="MultiWindowViewPresenterManager" />.
    /// </summary>
    /// <param name="rootFrame">The root frame.</param>
    public MultiWindowViewPresenterManager(IServiceProvider serviceProvider,
            ICrossWindowsFrame rootFrame, 
            ICrossViewModelLoader viewModelLoader,
            ICrossWindowsViewModelRequestTranslator requestTranslator, 
            ILogger<MultiWindowViewPresenterManager> logger)
        : base(logger)
    {
        _serviceProvider = serviceProvider;
        _requestTranslator = requestTranslator;
        _viewModelLoader = viewModelLoader;

        var window = (Microsoft.UI.Xaml.Application.Current as CrossWinUIApplication)?.MainWindow;
        if (window != null)
        {
            window.AppWindow.Closing += (_, __) => CloseAllWindows();
        }

        _mainFrame = new WindowInformation(window!, rootFrame, null);

        if (Window.Current != null)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += BackButtonOnBackRequested;
        }
    }
   
    /// <summary>
    ///     Creates a presentation attribute.
    /// </summary>
    /// <param name="viewModelType"></param>
    /// <param name="viewType"></param>
    /// <returns></returns>
    public override BasePresentationAttribute CreatePresentationAttribute(Type? viewModelType, Type? viewType)
    {
        Logger.LogInformation("PresentationAttribute not found for {ViewTypeName}. Assuming new page presentation",
            viewType?.Name);
        return new PagePresentationAttribute { ViewType = viewType, ViewModelType = viewModelType };
    }

    /// <summary>
    ///     Closes all windows, except the main window, and the view models belonging to those windows.
    /// </summary>
    public void CloseAllWindows()
    {
        List<WindowInformation> windows;
        lock (_windowInformation)
        {
            windows = _windowInformation.ToList();
        }

        foreach (var wi in windows)
        {
            try
            {
                CloseWindow(wi.Window);
            }
            catch (Exception)
            {
                // Swallow all exceptions.
            }

            wi.Window.Close();
        }
    }

    /// <summary>
    ///     Executed when the onBack is requested.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="backRequestedEventArgs">The event arguments.</param>
    protected virtual async void BackButtonOnBackRequested(object? sender, BackRequestedEventArgs backRequestedEventArgs)
    {
        if (backRequestedEventArgs.Handled)
        {
            return;
        }

        var currentView = GetWindowInformation(Window.Current).RootFrame.Content as ICrossView;
        if (currentView == null)
        {
            Logger?.LogWarning("Ignoring close for viewmodel - root frame has no current page");
            return;
        }

        if (currentView.ViewModel != null)
        {
            var navigationService = _serviceProvider.GetService<ICrossNavigationService>();
            backRequestedEventArgs.Handled = await navigationService!.Close(currentView.ViewModel);
        }
    }

    /// <summary>
    ///     Gets the correct root frame for the request.
    ///     There is no window or viewmodel for the original root frame.
    /// </summary>
    /// <param name="window">The window to get the root frame for.</param>
    /// <returns>The root frame, if no special root frame from a window is found the mainframe is returned.</returns>
    protected WindowInformation GetWindowInformation(Window window)
    {
        lock (_windowInformationLock)
        {
            return _windowInformation.Find(wi => wi.IsFor(window)) ?? _mainFrame;
        }
    }

    private void CloseWindow(Window newWindow)
    {
        var windowInformation = GetWindowInformation(newWindow);
        if (windowInformation.ViewModel != null)
        {
            Close(windowInformation.ViewModel);
        }
    }
}