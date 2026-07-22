namespace Nivaes.App.Cross.UIKitLib
{
    public sealed class PressenterActionContext 
        : IPressenterActionContext
    {
        public UIWindow Window { get; }

        public UINavigationController? MasterNavigationController { get; set; }

        public ITabBarViewController? TabBarViewController { get; set; }

        public IMvxSplitViewController? SplitViewController { get; set; }

        public IMvxPageViewController? PageViewController { get; set; }

#if IOS || MACCATALYST
        public UIViewController? PopoverViewController { get; set; }

        public SlideMenuViewController? SlideMenuController { get; set; }
#endif
        public UINavigationController? MainNavitagionController { get; set; }
        public IMenuViewController? MenuLeftViewController { get; set; }
        public IMenuViewController? MenuRigthViewController { get; set; }

        public List<UIViewController> ModalViewControllers { get; } = [];

        public List<UIMasterDetailSplitViewController> MasterDetailSplitViewControllers { get; } = [];

        public PressenterActionContext(UIWindow window)
        {
            Window = window;
        }
    }
}
