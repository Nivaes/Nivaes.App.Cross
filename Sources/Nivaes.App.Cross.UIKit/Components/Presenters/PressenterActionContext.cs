namespace Nivaes.App.Cross.UIKitLib
{
    public sealed class PressenterActionContext
    {
        public UIWindow Window { get; }

        public IMvxTabBarViewController? TabBarViewController { get; set; }

        public IMvxSplitViewController? SplitViewController { get; set; }

        public PressenterActionContext(UIWindow window)
        {
            Window = window;
        }
    }
}
