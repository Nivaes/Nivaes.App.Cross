namespace Nivaes.App.Cross.UIKitLib
{
    using System;
    using Foundation;
    using ObjCRuntime;

    public class NavigationController
        : UINavigationController
    {
        public NavigationController()
        {
        }

        public NavigationController(UIViewController rootViewController) : base(rootViewController)
        {
        }

        public NavigationController(NSCoder coder) : base(coder)
        {
        }

        public NavigationController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        public NavigationController(Type navigationBarType, Type toolbarType) : base(navigationBarType, toolbarType)
        {
        }

        protected NavigationController(NSObjectFlag t) : base(t)
        {
        }

        protected internal NavigationController(NativeHandle handle) : base(handle)
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
