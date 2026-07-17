namespace Nivaes.App.Cross.UIKitLib
{
    public interface IPressenterActionContext
    {
        UIWindow Window { get; }

        UINavigationController? MasterNavigationController { get; set; }

        ITabBarViewController? TabBarViewController { get; set; }

        IMvxSplitViewController? SplitViewController { get; set; }

        IMvxPageViewController? PageViewController { get; set; }

#if IOS || MACCATALYST
        UIViewController? PopoverViewController { get; set; }

        SlideMenuViewController? SlideMenuController { get; set; }
#endif
        UINavigationController? MainNavitagionController { get; set; }
        IMenuViewController? MenuLeftViewController { get; set; }
        IMenuViewController? MenuRigthViewController { get; set; }

        List<UIViewController> ModalViewControllers { get; }
        List <UIMasterDetailSplitViewController> MasterDetailSplitViewControllers { get; }
    }
}
