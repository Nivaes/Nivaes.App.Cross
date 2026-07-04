using Nivaes.App.Cross.UIKitLib;
using ObjCRuntime;

namespace Nivaes.App.Cross.Sample.UIKitLib;

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
