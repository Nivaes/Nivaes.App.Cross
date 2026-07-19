using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public sealed class SplitPressenterAction
            : PressenterAction<SplitViewPresentationAttribute>
    {
        #region Constructor
        public SplitPressenterAction(
                IPressenterActionContext context,
                IMvxIosViewCreator viewCreator,
                ILogger<SplitPressenterAction> logger)
            : base(context, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(Type viewType, SplitViewPresentationAttribute attribute, ViewModelRequest request)
        {
            var viewController = (UIViewController?)ViewCreator.CreateView(request);
            if (viewController == null)
            {
                Logger?.LogWarning(
                    "Got null ViewController for request {Request}", request);

                return ValueTask.FromResult(false);
            }

            var splitAttribute = attribute;
            return splitAttribute.Position switch
            {
                MasterDetailPosition.Master =>
                    ShowMasterSplitViewController(viewController, splitAttribute, request),
                MasterDetailPosition.Detail =>
                    ShowDetailSplitViewController(viewController, splitAttribute, request),
                _ => ValueTask.FromResult(true)
            };
        }

        protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, SplitViewPresentationAttribute attribute)
        {
            var splitAttribute = attribute;
            return splitAttribute.Position switch
            {
                MasterDetailPosition.Master => CloseMasterSplitViewController(viewModel, splitAttribute),
                MasterDetailPosition.Detail => CloseDetailSplitViewController(viewModel, splitAttribute),
                _ => CloseDetailSplitViewController(viewModel, splitAttribute)
            };
        }

        private ValueTask<bool> ShowDetailSplitViewController(
           UIViewController viewController,
           SplitViewPresentationAttribute attribute,
           ViewModelRequest request)
        {
            if (Context.SplitViewController == null)
                throw new AppException("Trying to show a detail page without a SplitViewController, this is not possible!");

            Context.SplitViewController.ShowDetailView(viewController, attribute);
            return ValueTask.FromResult(true);
        }

        private ValueTask<bool> CloseMasterSplitViewController(ICrossViewModel viewModel, SplitViewPresentationAttribute attribute)
        {
            if (Context.SplitViewController != null && Context.SplitViewController.CloseChildViewModel(viewModel, attribute))
                return ValueTask.FromResult(true);

            return ValueTask.FromResult(true);
        }

        private ValueTask<bool> CloseDetailSplitViewController(ICrossViewModel viewModel, SplitViewPresentationAttribute attribute)
        {
            if (Context.SplitViewController != null && Context.SplitViewController.CloseChildViewModel(viewModel, attribute))
                return ValueTask.FromResult(true);

            return ValueTask.FromResult(false);
        }
    }
}
