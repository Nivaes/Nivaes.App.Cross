namespace Nivaes.App.Cross.Sample.UIKitOS
{
    using Nivaes.App.Cross.UIKitOS;
    using ObjCRuntime;

    [MvxFromStoryboard("Main")]
    [MvxSplitViewPresentation(WrapInNavigationController = true)]
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
