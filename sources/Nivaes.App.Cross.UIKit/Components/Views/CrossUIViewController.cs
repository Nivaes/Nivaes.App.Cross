namespace Nivaes.App.Cross.UIKit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ObjCRuntime;

    public abstract class CrossUIViewController<TViewModel>
        : UIViewController,
        ICrossView<TViewModel>
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
