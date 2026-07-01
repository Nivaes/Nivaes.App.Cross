using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.UIKitOS;
using ObjCRuntime;

namespace Nivaes.App.Cross.Sample.UIKitOS;

[MvxFromStoryboard("Main")]
[MvxModalPresentation(WrapInNavigationController = true, ModalPresentationStyle = UIModalPresentationStyle.FormSheet)]
[RequiresUnreferencedCode("Bindings require unreferenced code")]
public partial class ModalNavView : MvxViewController<ModalNavViewModel>
{
    public ModalNavView(NativeHandle handle) : base(handle)
    {
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        View?.BackgroundColor = UIColor.Red;

        var set = CreateBindingSet();
        set.Bind(btnShowChild).To(vm => vm.ShowChildCommand);
        set.Bind(btnClose).To(vm => vm.CloseCommand);
        set.Bind(btnNestedModal).To(vm => vm.ShowNestedModalCommand);
        set.Apply();
    }
}
