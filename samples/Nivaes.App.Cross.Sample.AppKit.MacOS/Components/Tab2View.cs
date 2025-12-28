using MvvmCross.Platforms.Mac.Presenters.Attributes;
using Nivaes.App.Cross.AppKitOS;

namespace Nivaes.App.Cross.Sample.AppKitOS.MacOS;

[MvxTabPresentation(TabTitle = "Tab2")]
public class Tab2View 
    : CrossViewController<Tab2ViewModel>
{
    public Tab2View() 
        : base()
    {
    }

    public override void LoadView()
    {
        View = new AppKit.NSView(new CGRect(100, 100, 300, 300));
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        var lbl = new NSTextField(new CGRect(300, 300, 100, 30)) { Editable = false, Bezeled = false };
        lbl.StringValue = "Tab 2";

        View.AddSubview(lbl);
    }
}
