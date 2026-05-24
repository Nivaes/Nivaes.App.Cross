using System.Diagnostics.CodeAnalysis;
using MvvmCross.Platforms.Mac.Views;
using Nivaes.App.Cross.AppKitOS;
using ObjCRuntime;

namespace Nivaes.App.Cross.Sample.AppKitOS.MacOS;

[MvxFromStoryboard("Main")]
[CrossContentPresentation]
[RequiresUnreferencedCode("MvxBindings require unreferenced code")]
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
