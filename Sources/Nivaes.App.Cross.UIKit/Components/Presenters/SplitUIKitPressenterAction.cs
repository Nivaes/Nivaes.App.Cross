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

        protected override Task<bool> ShowAction(Type view, MvxSplitViewPresentationAttribute attribute, CrossViewModelRequest request)
        {
            var viewController = (UIViewController?)ViewCreator.CreateView(request);
            if (viewController == null)
            {
                Logger?.LogWarning(
                    "Got null ViewController for request {Request}", request);

                return Task.FromResult(false);
            }

            var splitAttribute = attribute;
            return splitAttribute.Position switch
            {
                MasterDetailPosition.Master =>
                    ShowMasterSplitViewController(viewController, splitAttribute, request),
                MasterDetailPosition.Detail =>
                    ShowDetailSplitViewController(viewController, splitAttribute, request),
                _ => Task.FromResult(true)
            };
        }

        protected override Task<bool> CloseAction(ICrossViewModel viewModel, MvxSplitViewPresentationAttribute attribute)
        {
            var splitAttribute = attribute;
            return splitAttribute.Position switch
            {
                MasterDetailPosition.Master => CloseMasterSplitViewController(viewModel, splitAttribute),
                MasterDetailPosition.Detail => CloseDetailSplitViewController(viewModel, splitAttribute),
                _ => CloseDetailSplitViewController(viewModel, splitAttribute)
            };
        }

        protected virtual Task<bool> ShowDetailSplitViewController(
           UIViewController viewController,
           MvxSplitViewPresentationAttribute attribute,
           CrossViewModelRequest request)
        {
            ArgumentNullException.ThrowIfNull(viewController);
            ArgumentNullException.ThrowIfNull(attribute);

            if (SplitViewController == null)
                throw new CrossException("Trying to show a detail page without a SplitViewController, this is not possible!");

            SplitViewController.ShowDetailView(viewController, attribute);
            return Task.FromResult(true);
        }

        protected virtual Task<bool> CloseMasterSplitViewController(ICrossViewModel viewModel, MvxSplitViewPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(viewModel);
            ArgumentNullException.ThrowIfNull(attribute);

            if (SplitViewController != null && SplitViewController.CloseChildViewModel(viewModel, attribute))
                return Task.FromResult(true);

            return Task.FromResult(true);
        }

        protected virtual Task<bool> CloseDetailSplitViewController(ICrossViewModel viewModel, MvxSplitViewPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(viewModel);
            ArgumentNullException.ThrowIfNull(attribute);

            if (SplitViewController != null && SplitViewController.CloseChildViewModel(viewModel, attribute))
                return Task.FromResult(true);

            return Task.FromResult(false);
        }
    }
}
