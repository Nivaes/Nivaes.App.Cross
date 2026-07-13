using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.AppKitLib;

namespace Nivaes.App.Cross.AppKitOS
{
    public sealed class WindowAppKitPressenterAction
        : AppKitPressenterAction<MvxWindowPresentationAttribute>
    {
        /// <summary>
        /// NSWindow keeps only the *weak* reference to its NSWindowController. So, the controller will be
        /// prematurely disposed if no other references exist. This table keeps a strong reference to the
        /// controller keeping it alive while the associated NSWindow is alive. Ref. issue #2198
        /// </summary>
        private readonly ConditionalWeakTable<NSWindow, NSWindowController> _windowsToWindowControllers = new();

        #region Constructor
        public WindowAppKitPressenterAction(
                ICrossViewsContainer viewsContainer,
                IMvxMacViewCreator viewCreator,
                ILogger<WindowAppKitPressenterAction> logger)
            : base(viewsContainer, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(Type viewType, MvxWindowPresentationAttribute attribute, CrossViewModelRequest request)
        {
            var viewController = (NSViewController)ViewCreator.CreateView(request);

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
            return ValueTask.FromResult(true);
        }

        private NSWindow CreateWindow(MvxWindowPresentationAttribute attribute)
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

        private MvxWindowController CreateWindowController(MvxWindowPresentationAttribute attribute)
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
                    throw new CrossException(
                        $"Could not determine window controller type for the {attribute.ViewModelType?.Name ?? "<unknown vm>"} view model. " +
                        $"Please specify either the {nameof(MvxWindowPresentationAttribute.WindowControllerType)} or " +
                        $"{nameof(MvxWindowPresentationAttribute.WindowControllerName)} property of the {nameof(MvxWindowPresentationAttribute)} " +
                        $"for the corresponding view model.");
                }
                // Instantiate using Reflection - failure is possible if blank constructor is missing
                windowController = (MvxWindowController?)Activator.CreateInstance(controllerType);
            }
            windowController!.ShouldCascadeWindows = attribute.ShouldCascadeWindows;
            return windowController;
        }

        private MvxWindowController CreateWindowController(NSWindow window)
        {
            return new MvxWindowController(window);
        }

        private void UpdateWindow(MvxWindowPresentationAttribute attribute, NSWindow window)
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
    }
}
