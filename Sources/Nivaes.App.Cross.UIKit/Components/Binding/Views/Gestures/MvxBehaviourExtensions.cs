namespace Nivaes.App.Cross.UIKitLib
{
    using UIKit;

    public static class MvxBehaviourExtensions
    {
        public static MvxTapGestureRecognizerBehaviour Tap(this UIView view, uint numberOfTapsRequired = 1,
                                                           uint numberOfTouchesRequired = 1,
                                                           bool cancelsTouchesInView = true)
        {
            var toReturn = new MvxTapGestureRecognizerBehaviour(view, numberOfTapsRequired, numberOfTouchesRequired, cancelsTouchesInView);
            return toReturn;
        }

        public static MvxSwipeGestureRecognizerBehaviour Swipe(this UIView view, UISwipeGestureRecognizerDirection direction,
                                                               uint numberOfTouchesRequired = 1)
        {
            var toReturn = new MvxSwipeGestureRecognizerBehaviour(view, direction, numberOfTouchesRequired);
            return toReturn;
        }
    }
}
