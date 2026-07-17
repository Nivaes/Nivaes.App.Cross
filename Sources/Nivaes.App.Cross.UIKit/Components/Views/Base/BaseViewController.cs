namespace Nivaes.App.Cross.UIKitLib
{

    /// <summary> A base view controller </summary>
    public abstract class BaseViewController<TViewModel>
        : MvxViewController<TViewModel>
        where TViewModel : IBaseViewModel
    {
        public BaseViewController()
            : base()
        {

        }

        protected BaseViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.Title = ViewModel!.Title;
            base.EdgesForExtendedLayout = UIKit.UIRectEdge.None;

            base.ViewDidLoad();

            InitKeyboardHandling();
        }

        #region Keyboard
        protected virtual bool HandlesKeyboardNotifications => false;
        protected virtual bool EnableAutoDismiss => false;

        /// <summary>
        /// The view to center on keyboard shown
        /// </summary>
        protected UIView? ViewToCenterOnKeyboardShown { get; set; }

        /// <summary>
        /// The scroll to center on keyboard shown
        /// </summary>
        protected UIScrollView? ScrollToCenterOnKeyboardShown { get; set; }

        private NSObject? mKeyboardShowObserver;
        private NSObject? mKeyboardHideObserver;

        /// <summary>
		/// Initialises the keyboard handling.  The view must also contain a UIScrollView for this to work.  You must also override HandlesKeyboardNotifications() and return true from that method.
		/// </summary>
		/// <param name="enableAutoDismiss">If set to <c>true</c> enable auto dismiss.</param>
		protected virtual void InitKeyboardHandling()
        {
            //Only do this if required
            if (HandlesKeyboardNotifications)
            {
                RegisterForKeyboardNotifications();
            }

            if (EnableAutoDismiss)
            {
                DismissKeyboardOnBackgroundTap();
            }
        }

        protected virtual void RegisterForKeyboardNotifications()
        {
#if IOS || MACCATALYST
            if (mKeyboardShowObserver == null)
            {
                mKeyboardShowObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardNotification);
            }

            if (mKeyboardHideObserver == null)
            {
                mKeyboardHideObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardNotification);
            }
#endif
        }

        protected virtual void UnregisterForKeyboardNotifications()
        {
            if (mKeyboardShowObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(mKeyboardShowObserver);
                mKeyboardShowObserver.Dispose();
                mKeyboardShowObserver = null;
            }

            if (mKeyboardHideObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(mKeyboardHideObserver);
                mKeyboardHideObserver.Dispose();
                mKeyboardHideObserver = null;
            }
        }

        /// <summary>
        /// Gets the UIView that represents the "active" user input control (e.g. textfield, or button under a text field)
        /// </summary>
        /// <returns>
        /// A <see cref="UIView"/>
        /// </returns>
        protected virtual UIView? KeyboardGetActiveView()
        {
            return View!.FindFirstResponder();
        }

        /// <summary>
        /// Called when keyboard notifications are produced.
        /// </summary>
        /// <param name="notification">The notification.</param>
        protected virtual void OnKeyboardNotification(NSNotification notification)
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification));

            if (!IsViewLoaded) return;

#if IOS || MACCATALYST
            //Check if the keyboard is becoming visible
            var visible = notification.Name == UIKeyboard.WillShowNotification;
#endif 
            //Start an animation, using values from the keyboard
            UIView.BeginAnimations("AnimateForKeyboard");
            UIView.SetAnimationBeginsFromCurrentState(true);
#if IOS 
            UIView.SetAnimationDuration(UIKeyboard.AnimationDurationFromNotification(notification));

            UIView.SetAnimationCurve((UIViewAnimationCurve)UIKeyboard.AnimationCurveFromNotification(notification));

            //Pass the notification, calculating keyboard height, etc.
            var keyboardFrame = visible
                ? UIKeyboard.FrameEndFromNotification(notification)
                : UIKeyboard.FrameBeginFromNotification(notification);
            OnKeyboardChanged(visible, keyboardFrame);
#endif
#if MACCATALYST || TVOS
            OnKeyboardChanged(true, CGRect.Empty);
#endif

            //Commit the animation
            UIView.CommitAnimations();
        }

        /// <summary>
        /// Override this method to apply custom logic when the keyboard is shown/hidden
        /// </summary>
        /// <param name='visible'>
        /// If the keyboard is visible
        /// </param>
        /// <param name='keyboardFrame'>
        /// Frame of the keyboard
        /// </param>
        protected virtual void OnKeyboardChanged(bool visible, CGRect keyboardFrame)
        {
            var activeView = ViewToCenterOnKeyboardShown ?? KeyboardGetActiveView();
            if (activeView == null)
            {
                return;
            }

            var scrollView = ScrollToCenterOnKeyboardShown ?? activeView.FindTopSuperviewOfType(View!, typeof(UIScrollView)) as UIScrollView;
            if (scrollView == null)
            {
                return;
            }

            if (!visible)
            {
                scrollView.RestoreScrollPosition();
            }
            else
            {
                scrollView.CenterView(activeView, keyboardFrame);
            }
        }

        /// <summary>
        /// Call it to force dismiss keyboard when background is tapped
        /// </summary>
        protected void DismissKeyboardOnBackgroundTap()
        {
            // Add gesture recognizer to hide keyboard
            var tap = new UITapGestureRecognizer { CancelsTouchesInView = false };
            tap.AddTarget(() => View!.EndEditing(true));
            tap.ShouldReceiveTouch = (recognizer, touch) => !(touch.View is UIControl || touch.View.FindSuperviewOfType(View, typeof(UITableViewCell)) != null);
            View!.AddGestureRecognizer(tap);
        }

        /// <summary>
        /// Selects next TextField to become FirstResponder.
        /// Usage: textField.ShouldReturn += TextFieldShouldReturn;
        /// </summary>
        /// <returns></returns>
        /// <param name="textField">The TextField</param>
        public bool TextFieldShouldReturn(UITextField textField)
        {
            if (textField == null) throw new ArgumentNullException(nameof(textField));

            var nextTag = textField.Tag + 1;
            UIResponder nextResponder = View!.ViewWithTag(nextTag);
            if (nextResponder != null)
            {
                nextResponder.BecomeFirstResponder();
            }
            else
            {
                // Not found, so remove keyboard.
                textField.ResignFirstResponder();
            }
            return false; // We do not want UITextField to insert line-breaks.
        }
        #endregion
    }
}
