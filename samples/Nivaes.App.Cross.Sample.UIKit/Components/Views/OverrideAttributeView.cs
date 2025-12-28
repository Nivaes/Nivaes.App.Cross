using System.Diagnostics.CodeAnalysis;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using Nivaes.App.Cross.UIKitOS;
using ObjCRuntime;

namespace Nivaes.App.Cross.Sample.UIKitOS;

[MvxFromStoryboard("Main")]
[RequiresUnreferencedCode("MvxBindings require unreferenced code")]
public partial class OverrideAttributeView 
    : MvxViewController<OverrideAttributeViewModel>, ICrossOverridePresentationAttribute
{
    public OverrideAttributeView(NativeHandle handle) : base(handle)
    {
    }

    public CrossBasePresentationAttribute PresentationAttribute(CrossViewModelRequest request)
    {
        return new MvxModalPresentationAttribute
        {
            ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen,
            ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve
        };
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        View?.BackgroundColor = UIColor.Cyan;

        var set = CreateBindingSet();
        set.Bind(btnTabs).To(vm => vm.ShowTabsCommand);
        set.Bind(btnClose).To(vm => vm.CloseCommand);
        set.Apply();
    }
}
