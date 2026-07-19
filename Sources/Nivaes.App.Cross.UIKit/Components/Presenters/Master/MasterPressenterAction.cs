#if IOS || MACCATALYST
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public sealed class MasterPressenterAction
            : PressenterAction<MasterPresentationAttribute>
    {
        #region Constructor
        public MasterPressenterAction(
                IPressenterActionContext context,
                IMvxIosViewCreator viewCreator,
                ILogger<MasterPressenterAction> logger)
            : base(context, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(Type viewType, MasterPresentationAttribute attribute, ViewModelRequest request)
        {
            var viewController = (UIViewController)ViewCreator.CreateView(request);
            return ShowMasterViewController(viewController, attribute, request);
        }

        protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, MasterPresentationAttribute attribute)
        {
            if (attribute.PanelType == PanelType.Secondary)
            {
                CloseMasterViewController(Context.MasterDetailSplitViewControllers.LastOrDefault());

                return ValueTask.FromResult(true);
            }

            return ValueTask.FromResult(false);
        }

        private ValueTask<bool> ShowMasterViewController(
           UIViewController viewController,
           MasterPresentationAttribute attribute,
           ViewModelRequest request)
        {
            if (viewController == null) throw new ArgumentNullException(nameof(viewController));
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));

            Context.MainNavitagionController = base.CreateNavigationController(viewController);

            var masterDetailSplitViewController = new UIMasterDetailSplitViewController
            {
                PreferredDisplayMode = UISplitViewControllerDisplayMode.AllVisible,
            };

            masterDetailSplitViewController.ShowMasterView(Context.MainNavitagionController);

            if (attribute.PanelType == PanelType.Primary)
            {
                Context.MasterDetailSplitViewControllers.Clear();

                CreateSlideMenuController(masterDetailSplitViewController);

                viewController.AddLeftBarButtonWithImage(UIImage.FromBundle("ic_menu"));
            }
            else if (attribute.PanelType == PanelType.Secondary)
            {
                masterDetailSplitViewController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
                masterDetailSplitViewController.ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve;

                //var modalHost = base.ModalViewControllers.LastOrDefault() ?? _window.RootViewController;
                var modalHost = Context.MasterDetailSplitViewControllers.LastOrDefault() ?? Context.Window.RootViewController;

                modalHost.PresentViewController(
                        masterDetailSplitViewController,
                        true,
                        null);

                Context.ModalViewControllers.Add(masterDetailSplitViewController);

                viewController.NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIImage.FromBundle("ic_back"), UIBarButtonItemStyle.Plain, (object sender, EventArgs e) =>
                {
                    CloseMasterViewController(Context.MasterDetailSplitViewControllers.LastOrDefault());
                });
            }

            Context.MasterDetailSplitViewControllers.Add(masterDetailSplitViewController);

            return ValueTask.FromResult(true);
        }

        private void CloseMasterViewController(UIMasterDetailSplitViewController splitViewController)
        {
            if (splitViewController == null)
                return;

            foreach (var viewController in splitViewController.ViewControllers)
            {
                if (viewController is UINavigationController modalNavController)
                {
                    foreach (var item in modalNavController.ViewControllers)
                        item.DidMoveToParentViewController(null);
                }
                else
                {
                    viewController.DidMoveToParentViewController(null);
                }
            }

            splitViewController.DismissViewController(true, null);

            Context.MasterDetailSplitViewControllers.Remove(splitViewController);
            Context.ModalViewControllers.Remove(splitViewController);
        }
    }
}
#endif