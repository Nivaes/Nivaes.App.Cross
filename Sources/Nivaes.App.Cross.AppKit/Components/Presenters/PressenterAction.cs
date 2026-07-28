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
                IMvxMacViewCreator viewCreator,
                ILogger logger)
            : base(logger)
        {
            Context = context;
            ViewCreator = viewCreator;
        }
        #endregion

        protected override BasePresentationAttribute CreatePresentationAttribute(IViewModelRequest request)
        {
            Logger.LogWarning($"PresentationAttribute not found for {request.ViewType.Name}. Assuming new window presentation", request.ViewType.Name);
            return new WindowPresentationAttribute();
        }

        protected override ValueTask<bool> CloseAction(IViewModelRequest request, TPressenterAttribute attribute)
        {
            for (int i = Context.Windows.Count - 1; i >= 0; i--)
            {
                var window = Context.Windows[i];

                // closing controller is a tab
                var tabViewController = window.ContentViewController as IMvxTabViewController;
                if (tabViewController != null && tabViewController.CloseTabView(request.ViewModel))
                {
                    return ValueTask.FromResult(true);
                }

                var controller = window.ContentViewController as ICrossViewController;

                // if closing controller is a sheet or modal, it must have a presenting parent
                var presentedController = controller!.PresentedViewControllers?.FirstOrDefault(c => ((ICrossView)c).ViewModel == request.ViewModel);
                if (presentedController != null)
                {
                    controller.DismissViewController(presentedController);
                    return ValueTask.FromResult(true);
                }

                // closing controller is content in a regular window
                if (controller != null && ((ICrossView)controller).ViewModel == request.ViewModel)
                {
                    Context.Windows.Remove(window);
                    window.Close();
                    return ValueTask.FromResult(true);
                }
            }

            throw new AppException($"Could not find and close a view for '{request.ViewModel.GetType()}'");
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
