#if IOS || MACCATALYST
using UIKit;

namespace Nivaes.App.Cross.UIKitLib
{
    public static class SlideMenuViewControllerExt
    {
        public static SlideMenuViewController SlideMenuController(this UIViewController controller)
        {
            while (controller != null)
            {
                if (controller is SlideMenuViewController)
                {
                    return controller as SlideMenuViewController;
                }

                controller = controller.ParentViewController;
            }

            return null;
        }

        public static void AddLeftBarButtonWithImage(this UIViewController controller, UIImage image)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));

            UIBarButtonItem leftBarButton = new UIBarButtonItem(image, UIBarButtonItemStyle.Plain, (object sender, EventArgs e) =>
            {
                controller.ToggleLeft();
            });

            controller.NavigationItem.LeftBarButtonItem = leftBarButton;
        }


        public static void AddRightBarButtonWithImage(this UIViewController controller, UIImage image)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));

            UIBarButtonItem rightBarButton = new UIBarButtonItem(image, UIBarButtonItemStyle.Plain, (object sender, EventArgs e) =>
            {
                controller.ToggleRight();
            });

            controller.NavigationItem.RightBarButtonItem = rightBarButton;
        }

        public static void ToggleLeft(this UIViewController controller)
        {
            SlideMenuViewController slideController = controller.SlideMenuController();

            slideController?.ToggleLeft();
        }

        public static void ToggleRight(this UIViewController controller)
        {
            SlideMenuViewController slideController = controller.SlideMenuController();

            slideController?.ToggleRight();
        }

        public static void OpenLeft(this UIViewController controller)
        {
            SlideMenuViewController slideController = controller.SlideMenuController();

            slideController?.OpenLeft();
        }

        public static void OpenRight(this UIViewController controller)
        {
            SlideMenuViewController slideController = controller.SlideMenuController();

            slideController?.OpenRight();
        }

        public static void CloseLeft(this UIViewController controller)
        {
            SlideMenuViewController slideController = controller.SlideMenuController();

            slideController?.CloseLeft();
        }

        public static void CloseRight(this UIViewController controller)
        {
            SlideMenuViewController slideController = controller.SlideMenuController();

            slideController?.CloseRight();
        }

        public static void AddPriorityToMenuGesuture(this UIViewController controller, UIScrollView targetScrollView)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));
            if (targetScrollView == null) throw new ArgumentNullException(nameof(targetScrollView));

            SlideMenuViewController slideController = controller.SlideMenuController();
            var recognizers = slideController.View.GestureRecognizers;

            if (slideController != null && recognizers != null)
            {
                foreach (UIGestureRecognizer gesture in recognizers)
                {
                    if (gesture is UIPanGestureRecognizer)
                    {
                        targetScrollView.PanGestureRecognizer.RequireGestureRecognizerToFail(gesture);
                    }
                }
            }
        }
    }
}
#endif