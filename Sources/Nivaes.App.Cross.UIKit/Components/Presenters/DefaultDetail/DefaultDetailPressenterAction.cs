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

        protected override ValueTask<bool> ShowAction(Type viewType, DefaultDetailPresentationAttribute attribute, ViewModelRequest request)
        {
            var viewController = (UIViewController)ViewCreator.CreateView(request);
            return ShowDefaultDetailViewController(viewController, (DefaultDetailPresentationAttribute)attribute, request);
        }

        protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, DefaultDetailPresentationAttribute attribute)
        {
            base.Logger.LogWarning($"Ignored attempt to close the window root (ViewModel type: {viewModel.GetType().Name}");

            return ValueTask.FromResult(false);
        }
        private ValueTask<bool> ShowDefaultDetailViewController(
         UIViewController viewController,
         DefaultDetailPresentationAttribute attribute,
         ViewModelRequest request)
        {
            Context.MasterNavigationController = base.CreateNavigationController(viewController);
            Context.MasterDetailSplitViewControllers.LastOrDefault()?.ShowDefaultDetailView(Context.MasterNavigationController);

            return ValueTask.FromResult(true);
        }
    }
}
#endif