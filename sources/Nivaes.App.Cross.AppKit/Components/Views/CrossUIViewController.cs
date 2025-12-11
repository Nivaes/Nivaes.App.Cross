namespace Nivaes.App.Cross.AppKit
{
    using ObjCRuntime;
    using System.ComponentModel;

    public abstract class CrossUIViewController<TViewModel>
        : NSViewController, ICrossView
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

        public ICrossViewModel? ViewModel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public object? DataContext { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
