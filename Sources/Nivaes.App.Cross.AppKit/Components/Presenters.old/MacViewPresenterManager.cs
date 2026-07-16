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

    public override BasePresentationAttribute CreatePresentationAttribute(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewType)
    {
        Logger.Log(LogLevel.Trace, $"PresentationAttribute not found for {viewType.Name}. Assuming new window presentation", viewType.Name);
        return new WindowPresentationAttribute { ViewModelType = viewModelType, ViewType = viewType };
    }

    [Obsolete("No usar Override", true)]
    public override BasePresentationAttribute GetOverridePresentationAttribute(
        CrossViewModelRequest request,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)] Type viewType)
    {
        if (viewType?.GetInterface(nameof(ICrossOverridePresentationAttribute)) != null)
        {
            var viewInstance = this.CreateViewControllerFor(viewType, null) as NSViewController;
            using (viewInstance)
            {
                var presentationAttribute = (viewInstance as ICrossOverridePresentationAttribute)?.PresentationAttribute(request);

                if (presentationAttribute == null)
                {
                    Logger.Log(LogLevel.Warning, "Override PresentationAttribute null. Falling back to existing attribute.");
                }
                else
                {
                    if (presentationAttribute.ViewType == null)
                        presentationAttribute.ViewType = viewType;

                    if (presentationAttribute.ViewModelType == null)
                        presentationAttribute.ViewModelType = request.ViewModelType;

                    return presentationAttribute;
                }
            }
        }

        return null;
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

    [Obsolete]
    public override void RegisterAttributeTypes()
    {
        AttributeTypesToActionsDictionary.Register<WindowPresentationAttribute>(
                (viewType, attribute, request) =>
                {
                    var viewController = (NSViewController)this.CreateViewControllerFor(request);
                    return ShowWindowViewController(viewController, (WindowPresentationAttribute)attribute, request);
                },
                (viewModel, attribute) => Close(viewModel));

        AttributeTypesToActionsDictionary.Register<ContentPresentationAttribute>(
                (viewType, attribute, request) =>
                {
                    var viewController = (NSViewController)this.CreateViewControllerFor(request);
                    return ShowContentViewController(viewController, (ContentPresentationAttribute)attribute, request);
                },
                (viewModel, attribute) => Close(viewModel));

        AttributeTypesToActionsDictionary.Register<ModalPresentationAttribute>(
                (viewType, attribute, request) =>
                {
                    var viewController = (NSViewController)this.CreateViewControllerFor(request);
                    return ShowModalViewController(viewController, (ModalPresentationAttribute)attribute, request);
                },
                (viewModel, attribute) => Close(viewModel));

        AttributeTypesToActionsDictionary.Register<SheetPresentationAttribute>(
                (viewType, attribute, request) =>
                {
                    var viewController = (NSViewController)this.CreateViewControllerFor(request);
                    return ShowSheetViewController(viewController, (SheetPresentationAttribute)attribute, request);
                },
                (viewModel, attribute) => Close(viewModel));

        AttributeTypesToActionsDictionary.Register<TabPresentationAttribute>(
                (viewType, attribute, request) =>
                {
                    var viewController = (NSViewController)this.CreateViewControllerFor(request);
                    return ShowTabViewController(viewController, (TabPresentationAttribute)attribute, request);
                },
                (viewModel, attribute) => Close(viewModel));
    }

    [Obsolete("User PressenterAction")]
    protected virtual Task<bool> ShowWindowViewController(
        NSViewController viewController,
        WindowPresentationAttribute attribute,
        CrossViewModelRequest request)
    {
        NSWindow window = null;
        MvxWindowController windowController = null;

        if (!string.IsNullOrEmpty(attribute.WindowControllerName))
        {
            windowController = CreateWindowController(attribute);
            window = windowController.Window;
        }

        if (window == null)
        {
            window = CreateWindow(attribute);

            if (windowController == null)
            {
                windowController = CreateWindowController(window);
                windowController.ShouldCascadeWindows = attribute.ShouldCascadeWindows;
            }
            windowController.Window = window;
        }
        else
        {
            UpdateWindow(attribute, window);
        }

        if (!Windows.Contains(window))
            Windows.Add(window);

        // ConditionalWeakTable automatically removes entries when the key (window) is garbage collected,
        // so we don't need to manually remove items when windows are closed
        _windowsToWindowControllers.AddOrUpdate(window, windowController);

        window.Identifier = attribute.Identifier ?? viewController.GetType().Name;

        if (!string.IsNullOrEmpty(viewController.Title))
            window.Title = viewController.Title;

        window.ContentView = viewController.View;
        window.ContentViewController = viewController;
        windowController.ShowWindow(null);
        return Task.FromResult(true);
    }

    [Obsolete("", true)]
    protected virtual void UpdateWindow(WindowPresentationAttribute attribute, NSWindow window)
    {
        var positionX = (float)window.Frame.X;
        var positionY = (float)window.Frame.Y;
        var width = (float)window.Frame.Width;
        var height = (float)window.Frame.Height;

        var newFrame = new CGRect(positionX, positionY, width, height);
        window.SetFrame(newFrame, false);

        window.StyleMask = attribute.WindowStyle;
        window.BackingType = attribute.BufferingType;
        window.TitleVisibility = attribute.TitleVisibility;
    }

    [Obsolete("", true)]
    protected virtual NSWindow CreateWindow(WindowPresentationAttribute attribute)
    {
        NSWindow window;
        var positionX = attribute.PositionX;
        var positionY = attribute.PositionY;
        var width = attribute.Width;
        var height = attribute.Height;

        window = new NSWindow(
            new CGRect(positionX, positionY, width, height),
            attribute.WindowStyle,
            attribute.BufferingType,
            false,
            NSScreen.MainScreen)
        {
            TitleVisibility = attribute.TitleVisibility,
        };
        return window;
    }

    [Obsolete("", true)]
    protected virtual MvxWindowController CreateWindowController(WindowPresentationAttribute attribute)
    {
        MvxWindowController? windowController;
        if (!string.IsNullOrEmpty(attribute.StoryboardName))
        {
            // Instantiate from storyboard
            var storyboard = NSStoryboard.FromName(attribute.StoryboardName, null);
            windowController = (MvxWindowController)storyboard.InstantiateControllerWithIdentifier(attribute.WindowControllerName);
        }
        else
        {
            var controllerType = attribute.WindowControllerType ?? Type.GetType(attribute.WindowControllerName);
            if (controllerType is null)
            {
                throw new AppException(
                    $"Could not determine window controller type for the {attribute.ViewModelType?.Name ?? "<unknown vm>"} view model. " +
                    $"Please specify either the {nameof(WindowPresentationAttribute.WindowControllerType)} or " +
                    $"{nameof(WindowPresentationAttribute.WindowControllerName)} property of the {nameof(WindowPresentationAttribute)} " +
                    $"for the corresponding view model.");
            }
            // Instantiate using Reflection - failure is possible if blank constructor is missing
            windowController = (MvxWindowController?)Activator.CreateInstance(controllerType);
        }
        windowController!.ShouldCascadeWindows = attribute.ShouldCascadeWindows;
        return windowController;
    }

    [Obsolete("", true)]
    protected virtual MvxWindowController CreateWindowController(NSWindow window)
    {
        return new MvxWindowController(window);
    }

    [Obsolete("User PressenterAction")]
    protected virtual Task<bool> ShowContentViewController(
        NSViewController viewController,
        ContentPresentationAttribute attribute,
        CrossViewModelRequest request)
    {
        var window = FindPresentingWindow(attribute.WindowIdentifier, viewController);

        if (!string.IsNullOrEmpty(viewController.Title))
            window.Title = viewController.Title;

        window.ContentView = viewController.View;
        window.ContentViewController = viewController;
        return Task.FromResult(true);
    }

    [Obsolete("User PressenterAction")]
    protected virtual Task<bool> ShowModalViewController(
        NSViewController viewController,
        ModalPresentationAttribute attribute,
        CrossViewModelRequest request)
    {
        var window = FindPresentingWindow(attribute.WindowIdentifier, viewController);

        window.ContentViewController.PresentViewControllerAsModalWindow(viewController);
        return Task.FromResult(true);
    }

    [Obsolete("User PressenterAction")]
    protected virtual Task<bool> ShowSheetViewController(
        NSViewController viewController,
        SheetPresentationAttribute attribute,
        CrossViewModelRequest request)
    {
        var window = FindPresentingWindow(attribute.WindowIdentifier, viewController);

        window.ContentViewController.PresentViewControllerAsSheet(viewController);
        return Task.FromResult(true);
    }

    [Obsolete("User PressenterAction")]
    protected virtual Task<bool> ShowTabViewController(
        NSViewController viewController,
        TabPresentationAttribute attribute,
        CrossViewModelRequest request)
    {
        var window = FindPresentingWindow(attribute.WindowIdentifier, viewController);

        if (window.ContentViewController is not IMvxTabViewController tabViewController)
            throw new AppException($"Trying to display a tab but there is no TabViewController to host it! View type: {viewController.GetType()}");

        tabViewController.ShowTabView(viewController, attribute.TabTitle);
        return Task.FromResult(true);
    }

    protected virtual NSWindow FindPresentingWindow(string identifier, NSViewController viewController)
    {
        NSWindow window = null;

        if (!string.IsNullOrEmpty(identifier))
            window = Windows.Find(w => w.Identifier == identifier);

        if (window == null)
            window = MainWindow ?? Windows.LastOrDefault();

        if (window == null)
            throw new AppException($"Could not find a window with identifier '{identifier}' to display view '{viewController.GetType()}'");

        return window;
    }

    [Obsolete("User PressenteAction", true)]
    public override Task<bool> Close(ICrossViewModel viewModel)
    {
        for (int i = Windows.Count - 1; i >= 0; i--)
        {
            var window = Windows[i];

            // closing controller is a tab
            var tabViewController = window.ContentViewController as IMvxTabViewController;
            if (tabViewController != null && tabViewController.CloseTabView(viewModel))
            {
                return Task.FromResult(true);
            }

            var controller = window.ContentViewController as ICrossViewController;

            // if closing controller is a sheet or modal, it must have a presenting parent
            var presentedController = controller!.PresentedViewControllers?.FirstOrDefault(c => ((ICrossView)c).ViewModel == viewModel);
            if (presentedController != null)
            {
                controller.DismissViewController(presentedController);
                return Task.FromResult(true);
            }

            // closing controller is content in a regular window
            if (controller != null && ((ICrossView)controller).ViewModel == viewModel)
            {
                Windows.Remove(window);
                window.Close();
                return Task.FromResult(true);
            }
        }

        throw new AppException($"Could not find and close a view for '{viewModel.GetType()}'");
    }

    protected void OnWindowWillCloseNotification(object? sender, NSNotificationEventArgs e)
    {
        var window = e.Notification.Object as NSWindow;
        if (Windows.Contains(window))
            Windows.Remove(window);
    }
}
