using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.AppKitLib;

namespace Nivaes.App.Cross.AppKitOS
{
    public abstract class AppKitPressenterAction<TPressenterAttribute>
                : PressenterAction<TPressenterAttribute>
        where TPressenterAttribute : IPresentationAttribute
    {
        // ToDo: Windows ha de ser una colección común para todos los PressenterAction.
        protected List<NSWindow> Windows { get; } = new List<NSWindow>();
        protected NSWindow MainWindow => NSApplication.SharedApplication.MainWindow;

        protected readonly IMvxMacViewCreator ViewCreator;

        #region Constructor
        protected AppKitPressenterAction(
                ICrossViewsContainer viewsContainer,
                IMvxMacViewCreator viewCreator,
                ILogger logger)
            : base(viewsContainer, logger)
        {
            ViewCreator = viewCreator;
        }
        #endregion

        protected override BasePresentationAttribute CreatePresentationAttribute(Type? viewModelType, Type? viewType)
        {
            Logger.LogTrace($"PresentationAttribute not found for {viewType.Name}. Assuming new window presentation", viewType.Name);
            return new WindowPresentationAttribute { ViewModelType = viewModelType, ViewType = viewType };
        }

        protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, TPressenterAttribute attribute)
        {
            for (int i = Windows.Count - 1; i >= 0; i--)
            {
                var window = Windows[i];

                // closing controller is a tab
                var tabViewController = window.ContentViewController as IMvxTabViewController;
                if (tabViewController != null && tabViewController.CloseTabView(viewModel))
                {
                    return ValueTask.FromResult(true);
                }

                var controller = window.ContentViewController as ICrossViewController;

                // if closing controller is a sheet or modal, it must have a presenting parent
                var presentedController = controller!.PresentedViewControllers?.FirstOrDefault(c => ((ICrossView)c).ViewModel == viewModel);
                if (presentedController != null)
                {
                    controller.DismissViewController(presentedController);
                    return ValueTask.FromResult(true);
                }

                // closing controller is content in a regular window
                if (controller != null && ((ICrossView)controller).ViewModel == viewModel)
                {
                    Windows.Remove(window);
                    window.Close();
                    return ValueTask.FromResult(true);
                }
            }

            throw new CrossException($"Could not find and close a view for '{viewModel.GetType()}'");
        }

        protected virtual NSWindow FindPresentingWindow(string identifier, NSViewController viewController)
        {
            NSWindow window = null;

            if (!string.IsNullOrEmpty(identifier))
                window = Windows.Find(w => w.Identifier == identifier);

            if (window == null)
                window = MainWindow ?? Windows.LastOrDefault();

            if (window == null)
                throw new CrossException($"Could not find a window with identifier '{identifier}' to display view '{viewController.GetType()}'");

            return window;
        }

    }
}
