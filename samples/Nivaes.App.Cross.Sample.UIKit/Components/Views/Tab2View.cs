using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.UIKitOS;
using ObjCRuntime;

namespace Nivaes.App.Cross.Sample.UIKitOS;

[MvxFromStoryboard("Main")]
[MvxTabPresentation]
[RequiresUnreferencedCode("MvxBindings require unreferenced code")]
public partial class Tab2View : MvxViewController<Tab2ViewModel>
{
    public Tab2View(NativeHandle handle) : base(handle)
    {
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        var set = CreateBindingSet();
        set.Bind(btnShowStack).To(vm => vm.ShowRootViewModelCommand);
        set.Bind(btnClose).To(vm => vm.CloseViewModelCommand);
        set.Apply();
    }
}
