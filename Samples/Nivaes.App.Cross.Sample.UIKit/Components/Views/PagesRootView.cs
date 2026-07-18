using Nivaes.App.Cross.UIKitLib;
using ObjCRuntime;

namespace Nivaes.App.Cross.Sample.UIKitLib
{
    [MvxFromStoryboard("Main")]
    [RootPresentation(WrapInNavigationController = true)]
    public partial class PagesRootView : PageViewController<PagesRootViewModel>
    {
        private bool _isPresentedFirstTime = true;

        public PagesRootView(NativeHandle handle) : base(handle)
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (ViewModel != null && _isPresentedFirstTime)
            {
                _isPresentedFirstTime = false;
                ViewModel.ShowInitialViewModelsCommand.ExecuteAsync(null);
            }
        }
    }
}
