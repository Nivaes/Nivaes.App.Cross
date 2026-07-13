using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public abstract class SplitUIKitPressenterAction
            : UIKitPressenterAction<MvxSplitViewPresentationAttribute>
    {
        #region Constructor
        public SplitUIKitPressenterAction(
                ICrossViewsContainer viewsContainer,
                IMvxIosViewCreator viewCreator,
                ILogger<SplitUIKitPressenterAction> logger)
            : base(viewsContainer, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(Type viewType, MvxSplitViewPresentationAttribute attribute, CrossViewModelRequest request)
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

        protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, MvxSplitViewPresentationAttribute attribute)
        {
            var splitAttribute = attribute;
            return splitAttribute.Position switch
            {
                MasterDetailPosition.Master => CloseMasterSplitViewController(viewModel, splitAttribute),
                MasterDetailPosition.Detail => CloseDetailSplitViewController(viewModel, splitAttribute),
                _ => CloseDetailSplitViewController(viewModel, splitAttribute)
            };
        }

        protected virtual ValueTask<bool> ShowDetailSplitViewController(
           UIViewController viewController,
           MvxSplitViewPresentationAttribute attribute,
           CrossViewModelRequest request)
        {
            if (SplitViewController == null)
                throw new CrossException("Trying to show a detail page without a SplitViewController, this is not possible!");

            SplitViewController.ShowDetailView(viewController, attribute);
            return ValueTask.FromResult(true);
        }

        protected virtual ValueTask<bool> CloseMasterSplitViewController(ICrossViewModel viewModel, MvxSplitViewPresentationAttribute attribute)
        {
            if (SplitViewController != null && SplitViewController.CloseChildViewModel(viewModel, attribute))
                return ValueTask.FromResult(true);

            return ValueTask.FromResult(true);
        }

        protected virtual ValueTask<bool> CloseDetailSplitViewController(ICrossViewModel viewModel, MvxSplitViewPresentationAttribute attribute)
        {
            if (SplitViewController != null && SplitViewController.CloseChildViewModel(viewModel, attribute))
                return ValueTask.FromResult(true);

            return ValueTask.FromResult(false);
        }
    }
}
