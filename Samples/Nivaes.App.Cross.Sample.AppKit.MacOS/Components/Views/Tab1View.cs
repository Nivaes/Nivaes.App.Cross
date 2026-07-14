using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.AppKitLib;
using ObjCRuntime;

namespace Nivaes.App.Cross.Sample.AppKitOS.MacOS;

[MvxFromStoryboard("Main")]
[TabPresentation(TabTitle = "Tab1")]
[RequiresUnreferencedCode("Bindings require unreferenced code")]
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
