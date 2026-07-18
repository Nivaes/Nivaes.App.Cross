#if IOS || MACCATALYST
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public sealed class DetailPressenterAction
            : PressenterAction<DetailPresentationAttribute>
    {
        #region Constructor
        public DetailPressenterAction(
                IPressenterActionContext context,
                IMvxIosViewCreator viewCreator,
                ILogger<DetailPressenterAction> logger)
            : base(context, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(Type viewType, DetailPresentationAttribute attribute, CrossViewModelRequest request)
        {
            var viewController = (UIViewController)ViewCreator.CreateView(request);
            return ShowDetailViewController(viewController, attribute, request);
        }

        protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, DetailPresentationAttribute attribute)
        {
            Logger.LogWarning($"Ignored attempt to close the window root (ViewModel type: {viewModel.GetType().Name}");

            return ValueTask.FromResult(false);
        }

        private ValueTask<bool> ShowDetailViewController(
          UIViewController viewController,
          DetailPresentationAttribute attribute,
          CrossViewModelRequest request)
        {
            Context.MasterNavigationController = base.CreateNavigationController(viewController);
            Context.MasterDetailSplitViewControllers.LastOrDefault()?.ShowDetailView(Context.MasterNavigationController);

            return ValueTask.FromResult(true);
        }
    }
}
#endif