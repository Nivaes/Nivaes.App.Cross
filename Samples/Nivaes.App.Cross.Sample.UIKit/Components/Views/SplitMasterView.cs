using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.UIKitLib;
using ObjCRuntime;

namespace Nivaes.App.Cross.Sample.UIKitLib;

[MvxFromStoryboard("Main")]
[SplitViewPresentation(MasterDetailPosition.Master)]
[RequiresUnreferencedCode("Bindings require unreferenced code")]
public partial class SplitMasterView : MvxViewController<SplitMasterViewModel>
{
    public SplitMasterView(NativeHandle handle) : base(handle)
    {
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        var set = CreateBindingSet();
        set.Bind(btnDetail).To(vm => vm.OpenDetailCommand);
        set.Bind(btnDetailNav).To(vm => vm.OpenDetailNavCommand);
        set.Bind(btnStack).To(vm => vm.ShowRootViewModel);
        set.Apply();
    }
}
