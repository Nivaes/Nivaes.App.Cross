namespace Playground.iOS.Views
{
    using Nivaes.App.Cross.UIKitOS;
    using ObjCRuntime;
    using Playground.Core.ViewModels;

    [MvxFromStoryboard("Main")]
    [MvxPagePresentation(WrapInNavigationController = false)]
    public partial class Page3View : MvxViewController<Page3ViewModel>
    {
        public Page3View(NativeHandle handle) : base(handle)
        {
        }
    }
}
