namespace Playground.iOS.Views
{
    using Nivaes.App.Cross.UIKitOS;
    using ObjCRuntime;
    using Playground.Core.ViewModels;

    [MvxFromStoryboard("Main")]
    [MvxRootPresentation(WrapInNavigationController = true)]
    public partial class PagesRootView : MvxPageViewController<PagesRootViewModel>
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
