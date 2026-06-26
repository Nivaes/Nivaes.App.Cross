using Nivaes.App.Cross.UIKitOS;
using ObjCRuntime;
using Playground.Core.ViewModels;

namespace Nivaes.App.Cross.Sample.UIKitOS
{  
    [MvxFromStoryboard("Main")]
    [MvxPagePresentation(WrapInNavigationController = false)]
    public partial class Page2View
        : MvxViewController<Page2ViewModel>
    {
        public Page2View(NativeHandle handle) : base(handle)
        {
        }
    }
}
