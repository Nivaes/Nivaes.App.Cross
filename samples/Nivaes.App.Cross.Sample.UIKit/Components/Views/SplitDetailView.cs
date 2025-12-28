using Nivaes.App.Cross.UIKitOS;
using ObjCRuntime;

namespace Nivaes.App.Cross.Sample.UIKitOS;

[MvxFromStoryboard("Main")]
[MvxSplitViewPresentation]
public partial class SplitDetailView : MvxViewController<SplitDetailViewModel>
{
    public SplitDetailView(NativeHandle handle) : base(handle)
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
