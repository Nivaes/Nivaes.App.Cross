namespace Nivaes.App.Cross.Sample.UIKitLib
{
    using Nivaes.App.Cross.UIKitLib;
    using ObjCRuntime;

    [MvxFromStoryboard("Main")]
    [SplitViewPresentation(WrapInNavigationController = true)]
    public partial class SplitDetailNavView : MvxViewController<SplitDetailNavViewModel>
    {
        public SplitDetailNavView(NativeHandle handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            btnClose.TouchUpInside += (sender, e) =>
            {
                DismissViewController(true, null);
            };
        }
    }
}
