namespace Nivaes.App.Cross.UIKit
{
    using ObjCRuntime;

    public class CrossNavigationController : UINavigationController
    {
        public CrossNavigationController()
        {
        }

        public CrossNavigationController(UIViewController rootViewController) : base(rootViewController)
        {
        }

        public CrossNavigationController(NSCoder coder) : base(coder)
        {
        }

        public CrossNavigationController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        public CrossNavigationController(Type navigationBarType, Type toolbarType) : base(navigationBarType, toolbarType)
        {
        }

        protected CrossNavigationController(NSObjectFlag t) : base(t)
        {
        }

        protected internal CrossNavigationController(NativeHandle handle) : base(handle)
        {
        }

        public override void PushViewController(UIViewController viewController, bool animated)
        {
            base.PushViewController(viewController, animated);
        }

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
    }
}
