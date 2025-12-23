namespace Nivaes.App.Cross.UIKit
{
    using UIKit;

    public class MvxTapGestureRecognizerBehaviour
        : MvxGestureRecognizerBehavior<UITapGestureRecognizer>
    {
        protected override void HandleGesture(UITapGestureRecognizer gesture)
        {
            FireCommand();
        }

        public MvxTapGestureRecognizerBehaviour(UIView target, uint numberOfTapsRequired = 1,
                                                uint numberOfTouchesRequired = 1,
                                                bool cancelsTouchesInView = true)
        {
            var tap = new UITapGestureRecognizer(HandleGesture)
            {
                NumberOfTapsRequired = numberOfTapsRequired,
#if IOS || MACCATALYST
                NumberOfTouchesRequired = numberOfTouchesRequired,
#endif
                CancelsTouchesInView = cancelsTouchesInView
            };

            AddGestureRecognizer(target, tap);
        }
    }
}
