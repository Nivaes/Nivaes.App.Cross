namespace Nivaes.App.Cross.UIKitLib
{
    public class UIMasterDetailSplitViewController
        : UISplitViewController
    {
        private static UIUserInterfaceIdiom mUserInterfaceIdiom = UIDevice.CurrentDevice.UserInterfaceIdiom;

        private UIMasterDetailSplitViewControllerDelegate mUIMasterDetailSplitViewControllerDelegate;

        public UIMasterDetailSplitViewController()
        {
            mUIMasterDetailSplitViewControllerDelegate = new UIMasterDetailSplitViewControllerDelegate();

            base.Delegate = mUIMasterDetailSplitViewControllerDelegate;
        }

        public void ShowMasterView(UIViewController viewController)
        {
            mUIMasterDetailSplitViewControllerDelegate.PrimaryViewControler = viewController;

            var defaultView = mUIMasterDetailSplitViewControllerDelegate.DefaultViewControler;
            if (defaultView == null)
                base.ViewControllers = new UIViewController[] { viewController };
            else
                base.ViewControllers = new UIViewController[] { viewController, defaultView };
        }

        public void ShowDetailView(UIViewController viewController)
        {
            base.ShowDetailViewController(viewController, this);
        }

        public void ShowDefaultDetailView(UIViewController viewController)
        {
            mUIMasterDetailSplitViewControllerDelegate.DefaultViewControler = viewController;

            base.ShowDetailViewController(viewController, this);
        }

        private class UIMasterDetailSplitViewControllerDelegate
            : UISplitViewControllerDelegate
        {
            public UIViewController? PrimaryViewControler;

            public UIViewController? DefaultViewControler;

            public override bool CollapseSecondViewController(UISplitViewController splitViewController, UIViewController secondaryViewController, UIViewController primaryViewController)
            {


                return true;
                //var v = DefaultViewControler == secondaryViewController;
                //return v;
            }

            public override bool EventShowDetailViewController(UISplitViewController splitViewController, UIViewController vc, NSObject sender)
            {
                if (vc == PrimaryViewControler)
                {

                }
                else if (vc == DefaultViewControler && splitViewController.Collapsed)
                {
                    return true;
                }

                return false;
            }

            public override bool EventShowViewController(UISplitViewController splitViewController, UIViewController vc, NSObject sender)
            {
                return true;
            }

            public override UIViewController SeparateSecondaryViewController(UISplitViewController splitViewController, UIViewController primaryViewController)
            {
                return DefaultViewControler!;
            }

#if IOS || MACCATALYST
            public override bool ShouldHideViewController(UISplitViewController svc, UIViewController viewController, UIInterfaceOrientation inOrientation)
            {
                if (viewController == PrimaryViewControler)
                {

                }
                else if (viewController == DefaultViewControler)
                {
                }


                return false;
            }
#endif

            public override UIViewController GetPrimaryViewControllerForCollapsingSplitViewController(UISplitViewController splitViewController)
            {
                return PrimaryViewControler!;
            }

            public override UIViewController GetPrimaryViewControllerForExpandingSplitViewController(UISplitViewController splitViewController)
            {
                return PrimaryViewControler!;
            }
        }
    }
}
