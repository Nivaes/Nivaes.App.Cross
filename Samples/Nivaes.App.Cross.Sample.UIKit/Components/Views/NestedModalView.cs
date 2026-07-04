using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.UIKitLib;
using ObjCRuntime;

namespace Nivaes.App.Cross.Sample.UIKitLib;

[MvxFromStoryboard("Main")]
[MvxModalPresentation(WrapInNavigationController = true)]
[RequiresUnreferencedCode("Bindings require unreferenced code")]
public partial class NestedModalView : MvxViewController<NestedModalViewModel>
{
    public NestedModalView(NativeHandle handle) : base(handle)
    {
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        View?.BackgroundColor = UIColor.Orange;

        var set = CreateBindingSet();
        set.Bind(btnTabs).To(vm => vm.ShowTabsCommand);
        set.Bind(btnClose).To(vm => vm.CloseCommand);
        set.Apply();
    }
}
