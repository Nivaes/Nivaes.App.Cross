namespace Nivaes.App.Cross.AppKit
{
    using ObjCRuntime;
    using System.ComponentModel;

    public abstract class CrossUIViewController<TViewModel>
        : NSViewController, IView
        where TViewModel : class, ICrossViewModel
    {
        public CrossUIViewController() 
            : base()
        {
        }
      
        public CrossUIViewController(NSCoder coder) 
            : base(coder)
        {
        }

        public CrossUIViewController(string? nibNameOrNull, NSBundle? nibBundleOrNull)
            : base(nibNameOrNull, nibBundleOrNull)
        {
        }

        protected CrossUIViewController(NSObjectFlag t)
            : base(t)
        {
        }
       
        protected internal CrossUIViewController(NativeHandle handle)
            : base(handle)
        {
        }
    }
}
