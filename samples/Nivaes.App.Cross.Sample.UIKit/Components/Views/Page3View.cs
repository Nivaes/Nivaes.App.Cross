namespace Playground.iOS.Views
{
    using MvvmCross.Platforms.Ios.Presenters.Attributes;
    using MvvmCross.Platforms.Ios.Views;
    using Nivaes.App.Cross.UIKit;
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
