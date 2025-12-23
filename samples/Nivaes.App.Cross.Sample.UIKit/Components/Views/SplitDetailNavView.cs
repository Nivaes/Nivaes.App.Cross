namespace Playground.iOS.Views
{
    using System;
    using MvvmCross.Platforms.Ios.Presenters.Attributes;
    using MvvmCross.Platforms.Ios.Views;
    using Nivaes.App.Cross.UIKit;
    using ObjCRuntime;
    using Playground.Core.ViewModels;

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
