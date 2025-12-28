namespace Nivaes.App.Cross.UIKitOS
{
    using System;
    using Foundation;
    using ObjCRuntime;

    public class MvxNavigationController 
        : UINavigationController
    {
        public MvxNavigationController()
        {
        }

        public MvxNavigationController(UIViewController rootViewController) : base(rootViewController)
        {
        }

        public MvxNavigationController(NSCoder coder) : base(coder)
        {
        }

        public MvxNavigationController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        public MvxNavigationController(Type navigationBarType, Type toolbarType) : base(navigationBarType, toolbarType)
        {
        }

        protected MvxNavigationController(NSObjectFlag t) : base(t)
        {
        }

        protected internal MvxNavigationController(NativeHandle handle) : base(handle)
        {
        }

        public override void PushViewController(UIViewController viewController, bool animated)
        {
            base.PushViewController(viewController, animated);
        }

#if IOS || MACCATALYST
        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            return TopViewController?.GetSupportedInterfaceOrientations() ?? base.GetSupportedInterfaceOrientations();
        }

        public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation()
        {
            return TopViewController?.PreferredInterfaceOrientationForPresentation() ?? base.PreferredInterfaceOrientationForPresentation();
        }

        public override bool ShouldAutorotate()
        {
            return TopViewController?.ShouldAutorotate() ?? base.ShouldAutorotate();
        }
#endif
    }
}
