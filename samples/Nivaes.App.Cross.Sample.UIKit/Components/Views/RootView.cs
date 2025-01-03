namespace Nivaes.App.Cross.UIKit.Sample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Sample;
    using ObjCRuntime;

    public partial class RootView
        : CrossUIViewController<RootViewModel>
    {
        public RootView(NativeHandle handle)
            : base(handle)
        {
        }
    }
}
