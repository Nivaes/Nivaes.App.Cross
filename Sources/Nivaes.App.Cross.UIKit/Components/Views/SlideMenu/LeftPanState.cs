namespace Nivaes.iOS.Core
{
    using CoreGraphics;
    using UIKit;

    struct LeftPanState
    {
        public static CGRect FrameAtStartOfPan = new CGRect(0, 0, 0, 0);
        public static CGPoint StartPointOfPan = new CGPoint(0, 0);
        public static bool WasOpenAtStartOfPan = false;
        public static bool WasHiddenAtStartOfPan = false;
        public static UIGestureRecognizerState LastState = UIGestureRecognizerState.Ended;
    }
}
