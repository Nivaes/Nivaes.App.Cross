namespace Nivaes.App.Cross.UIKitLib
{
    public sealed class PressenterActionContext
    {
        public UIWindow Window { get; }

        public IMvxTabBarViewController? TabBarViewController;

        public IMvxSplitViewController? SplitViewController;

#if IOS || MACCATALYST
        public SlideMenuViewController? SlideMenuController;
#endif
        public UINavigationController? MainNavitagionController;
        public IMenuViewController? MenuLeftViewController;
        public IMenuViewController? MenuRigthViewController;
        public List<UIMasterDetailSplitViewController> MasterDetailSplitViewControllers = new();

        public PressenterActionContext(UIWindow window)
        {
            Window = window;
        }
    }
}
