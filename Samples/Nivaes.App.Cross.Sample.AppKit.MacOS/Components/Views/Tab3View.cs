using MvvmCross.Platforms.Mac.Presenters.Attributes;
using Nivaes.App.Cross.AppKitOS;

namespace Nivaes.App.Cross.Sample.AppKitOS.MacOS
{
    [MvxTabPresentation(TabTitle = "Tab3")]
    public class Tab3View
        : CrossViewController<Tab3ViewModel>
    {
        public Tab3View()
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
        }
    }
}
