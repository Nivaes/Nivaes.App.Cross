namespace Nivaes.App.Cross.UIKitLib
{
    [MvxFromStoryboard("InitializingAppView")]
    [RootPresentation(WrapInNavigationController = false)]
    public partial class InitializingAppView :
        MvxViewController<InitializingAppViewModel>
    {
        public InitializingAppView(IntPtr handle)
            : base(handle)
        {
        }
    }
}
