namespace Nivaes.App.Cross.UIKit
{
    using UIKit;

    public static class CrossBehaviourExtensions
    {
        public static CrossTapGestureRecognizerBehaviour Tap(this UIView view, uint numberOfTapsRequired = 1,
                                                           uint numberOfTouchesRequired = 1,
                                                           bool cancelsTouchesInView = true)
        {
            var toReturn = new CrossTapGestureRecognizerBehaviour(view, numberOfTapsRequired, numberOfTouchesRequired, cancelsTouchesInView);
            return toReturn;
        }

        public static CrossSwipeGestureRecognizerBehaviour Swipe(this UIView view, UISwipeGestureRecognizerDirection direction,
                                                               uint numberOfTouchesRequired = 1)
        {
            var toReturn = new CrossSwipeGestureRecognizerBehaviour(view, direction, numberOfTouchesRequired);
            return toReturn;
        }
    }
}
