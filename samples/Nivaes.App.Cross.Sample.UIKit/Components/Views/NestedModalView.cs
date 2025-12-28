using System.Diagnostics.CodeAnalysis;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using Nivaes.App.Cross.UIKitOS;
using ObjCRuntime;

namespace Nivaes.App.Cross.Sample.UIKitOS;

[MvxFromStoryboard("Main")]
[MvxModalPresentation(WrapInNavigationController = true)]
[RequiresUnreferencedCode("MvxBindings require unreferenced code")]
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
