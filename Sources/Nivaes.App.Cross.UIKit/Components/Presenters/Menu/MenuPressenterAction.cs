#if IOS || MACCATALYST
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public sealed class MenuPressenterAction
            : PressenterAction<MenuPresentationAttribute>
    {
        #region Constructor
        public MenuPressenterAction(
                IPressenterActionContext context,
                IMvxIosViewCreator viewCreator,
                ILogger<MenuPressenterAction> logger)
            : base(context, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(IViewModelRequest request, MenuPresentationAttribute attribute)
        {
            var viewController = (UIViewController)ViewCreator.CreateView(request);
            return ShowMenuViewController(viewController, attribute, request);
        }

        protected override ValueTask<bool> CloseAction(IViewModelRequest request, MenuPresentationAttribute attribute)
        {
            Logger.LogWarning($"Ignored attempt to close the window root (ViewModel type: {request.ViewModelType.Name}");

            return ValueTask.FromResult(false);
        }

        private ValueTask<bool> ShowMenuViewController(
                UIViewController viewController,
                MenuPresentationAttribute attribute,
                IViewModelRequest request)
        {
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));

            if (attribute.MenuPosition == MenuPanelPosition.Left)
                Context.MenuLeftViewController = viewController as IMenuViewController;
            else if (attribute.MenuPosition == MenuPanelPosition.Rigth)
                Context.MenuRigthViewController = viewController as IMenuViewController;

            SetWindowSlideMenuViewController();

            return ValueTask.FromResult(true);
        }
    }
}
#endif