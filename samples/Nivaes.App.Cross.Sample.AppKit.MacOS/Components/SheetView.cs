using System.Diagnostics.CodeAnalysis;
using MvvmCross.Platforms.Mac.Presenters.Attributes;
using Nivaes.App.Cross.AppKitOS;
using ObjCRuntime;

namespace Nivaes.App.Cross.Sample.AppKitOS.MacOS;

[MvxFromStoryboard("Main")]
[MvxSheetPresentation]
[RequiresUnreferencedCode("MvxBindings require unreferenced code")]
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
