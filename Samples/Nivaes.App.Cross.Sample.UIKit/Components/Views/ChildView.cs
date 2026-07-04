using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.UIKitLib;
using ObjCRuntime;

namespace Nivaes.App.Cross.Sample.UIKitLib;

[MvxFromStoryboard("Main")]
[MvxChildPresentation]
[RequiresUnreferencedCode("Bindings require unreferenced code")]
public partial class ChildView
    : MvxViewController<ChildViewModel>
{
    public ChildView(NativeHandle handle) : base(handle)
    {
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        View?.BackgroundColor = UIColor.Yellow;

        var set = CreateBindingSet();
        set.Bind(btnClose).To(vm => vm.CloseCommand);
        set.Bind(btnShowSecondChild).To(vm => vm.ShowSecondChildCommand);
        set.Apply();
    }
}
