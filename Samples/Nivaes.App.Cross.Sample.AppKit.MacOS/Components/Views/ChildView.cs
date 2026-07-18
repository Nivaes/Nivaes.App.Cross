using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.AppKitLib;
using ObjCRuntime;

namespace Nivaes.App.Cross.Sample.AppKitOS.MacOS;

[MvxFromStoryboard("Main")]
[ContentPresentation]
public partial class ChildView
    : CrossViewController<ChildViewModel>
{
    public ChildView(NativeHandle handle) : base(handle)
    {
        Title = "Child view";
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        var set = CreateBindingSet();
        set.Bind(btnRoot).To(vm => vm.ShowRootCommand);
        set.Apply();
    }
}
