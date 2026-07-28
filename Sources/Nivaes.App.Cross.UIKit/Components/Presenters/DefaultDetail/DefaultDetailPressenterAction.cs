#if IOS || MACCATALYST
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public sealed class DefaultDetailPressenterAction
            : PressenterAction<DefaultDetailPresentationAttribute>
    {
        #region Constructor
        public DefaultDetailPressenterAction(
                IPressenterActionContext context,
                IMvxIosViewCreator viewCreator,
                ILogger<DefaultDetailPressenterAction> logger)
            : base(context, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(IViewModelRequest request, DefaultDetailPresentationAttribute attribute)
        {
            var viewController = (UIViewController)ViewCreator.CreateView(request);
            return ShowDefaultDetailViewController(viewController, (DefaultDetailPresentationAttribute)attribute, request);
        }

        protected override ValueTask<bool> CloseAction(IViewModelRequest request, DefaultDetailPresentationAttribute attribute)
        {
            Logger.LogWarning($"Ignored attempt to close the window root (ViewModel type: {request.ViewModelType.Name}");

            return ValueTask.FromResult(false);
        }

        private ValueTask<bool> ShowDefaultDetailViewController(
                UIViewController viewController,
                DefaultDetailPresentationAttribute attribute,
                IViewModelRequest request)
        {
            Context.MasterNavigationController = base.CreateNavigationController(viewController);
            Context.MasterDetailSplitViewControllers.LastOrDefault()?.ShowDefaultDetailView(Context.MasterNavigationController);

            return ValueTask.FromResult(true);
        }
    }
}
#endif