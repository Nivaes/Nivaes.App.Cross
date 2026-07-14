using Nivaes.App.Cross.UIKitLib;
using ObjCRuntime;

namespace Nivaes.App.Cross.Sample.UIKitLib
{
    [MvxFromStoryboard("Main")]
    [RootPresentation]
    public partial class SplitRootView : MvxSplitViewController<SplitRootViewModel>
    {
        private bool _isPresentedFirstTime = true;

        public SplitRootView(NativeHandle handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            PreferredPrimaryColumnWidthFraction = .3f;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (ViewModel != null && _isPresentedFirstTime)
            {
                _isPresentedFirstTime = false;
                ViewModel.ShowInitialMenuCommand.Execute(null);

                //PreferredDisplayMode = UISplitViewControllerDisplayMode.PrimaryHidden;
            }
        }
    }
}
