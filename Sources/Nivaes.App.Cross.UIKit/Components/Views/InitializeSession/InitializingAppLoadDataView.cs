namespace Nivaes.App.Cross.UIKitLib
{
    [MvxFromStoryboard("InitializingAppLoadDataView")]
    [RootPresentation(WrapInNavigationController = false)]
    public partial class InitializingAppLoadDataView :
        MvxViewController<InitializingAppLoadDataViewModel>
    {
        public InitializingAppLoadDataView(IntPtr handle)
            : base(handle)
        {
        }
    }
}
