namespace Nivaes.App.Cross.UIKit.Sample
{
    using Nivaes.App.Cross.AppKit;
    using Nivaes.App.Cross.Sample;
    using ObjCRuntime;

    public class RootView
        : CrossUIViewController<RootViewModel>
    {
        public RootView()
             : base()
        {
        }

        public RootView(NSCoder coder)
            : base(coder)
        {
        }

        public RootView(string? nibNameOrNull, NSBundle? nibBundleOrNull)
            : base(nibNameOrNull, nibBundleOrNull)
        {
        }

        public RootView(NativeHandle handle) : base(handle)
        {
        }
    }
}
