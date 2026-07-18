using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.AppKitLib
{
    public abstract class PressenterAction<TPressenterAttribute>
                : Cross.PressenterAction<TPressenterAttribute>
        where TPressenterAttribute : IPresentationAttribute
    {
        protected readonly IPressenterActionContext Context;

        protected readonly IMvxMacViewCreator ViewCreator;

        #region Constructor
        protected PressenterAction(
                IPressenterActionContext context,
                ICrossViewsContainer viewsContainer,
                IMvxMacViewCreator viewCreator,
                ILogger logger)
            : base(viewsContainer, logger)
        {
            Context = context;
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
            for (int i = Context.Windows.Count - 1; i >= 0; i--)
            {
                var window = Context.Windows[i];

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
                    Context.Windows.Remove(window);
                    window.Close();
                    return ValueTask.FromResult(true);
                }
            }

            throw new AppException($"Could not find and close a view for '{viewModel.GetType()}'");
        }

        protected virtual NSWindow FindPresentingWindow(string identifier, NSViewController viewController)
        {
            NSWindow? window = null;

            if (!string.IsNullOrEmpty(identifier))
                window = Context.Windows.Find(w => w.Identifier == identifier);

            if (window == null)
                window = Context.MainWindow ?? Context.Windows.LastOrDefault();

            if (window == null)
                throw new AppException($"Could not find a window with identifier '{identifier}' to display view '{viewController.GetType()}'");

            return window;
        }

    }
}
