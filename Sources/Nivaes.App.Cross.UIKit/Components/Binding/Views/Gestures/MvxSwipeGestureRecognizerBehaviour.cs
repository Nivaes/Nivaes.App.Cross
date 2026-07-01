namespace Nivaes.App.Cross.UIKitOS
{
    public class MvxSwipeGestureRecognizerBehaviour
        : MvxGestureRecognizerBehavior<UISwipeGestureRecognizer>
    {
        protected override void HandleGesture(UISwipeGestureRecognizer gesture)
        {
            FireCommand();
        }

        public MvxSwipeGestureRecognizerBehaviour(UIView target, UISwipeGestureRecognizerDirection direction,
                                                uint numberOfTouchesRequired = 1)
        {
            var swipe = new UISwipeGestureRecognizer(HandleGesture)
            {
                Direction = direction,
#if IOS || MACCATALYST
                NumberOfTouchesRequired = numberOfTouchesRequired
#endif
            };

            AddGestureRecognizer(target, swipe);
        }
    }
}
