using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.AppKitLib;
using ObjCRuntime;

namespace Nivaes.App.Cross.Sample.AppKitOS.MacOS;

[MvxFromStoryboard("Main")]
[MvxSheetPresentation]
[RequiresUnreferencedCode("Bindings require unreferenced code")]
public partial class SheetView : CrossViewController<SheetViewModel>
{
    public SheetView(NativeHandle handle) : base(handle)
    {
        Title = "Sheet view";
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        var set = CreateBindingSet();
        set.Bind(btnClose).To(vm => vm.CloseCommand);
        set.Apply();
    }
}
