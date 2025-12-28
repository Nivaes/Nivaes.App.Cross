using System.Diagnostics.CodeAnalysis;
using MvvmCross.Platforms.Mac.Presenters.Attributes;
using Nivaes.App.Cross.AppKitOS;
using ObjCRuntime;

namespace Nivaes.App.Cross.Sample.AppKitOS.MacOS;

[MvxFromStoryboard("Main")]
[MvxTabPresentation(TabTitle = "Tab1")]
[RequiresUnreferencedCode("MvxBindings require unreferenced code")]
public partial class Tab1View : CrossViewController<Tab1ViewModel>
{
    public Tab1View(NativeHandle handle) : base(handle)
    {
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        var set = CreateBindingSet();
        set.Bind(btnClose).To(vm => vm.CloseCommand);
        set.Apply();
    }
}
