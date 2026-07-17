#if IOS || MACCATALYST
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public sealed class MenuPressenterAction
            : PressenterAction<MenuPresentationAttribute>
    {
        #region Constructor
        public MenuPressenterAction(
                PressenterActionContext context,
                ICrossViewsContainer viewsContainer,
                IMvxIosViewCreator viewCreator,
                ILogger<MenuPressenterAction> logger)
            : base(context, viewsContainer, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(Type viewType, MenuPresentationAttribute attribute, CrossViewModelRequest request)
        {
            var viewController = (UIViewController)ViewCreator.CreateView(request);
            return ShowMenuViewController(viewController, attribute, request);
        }

        protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, MenuPresentationAttribute attribute)
        {
            base.Logger.LogWarning($"Ignored attempt to close the window root (ViewModel type: {viewModel.GetType().Name}");

            return ValueTask.FromResult(false);
        }

        private ValueTask<bool> ShowMenuViewController(
                UIViewController viewController,
                MenuPresentationAttribute attribute,
                CrossViewModelRequest request)
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