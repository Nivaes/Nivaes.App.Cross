using Nivaes.App.Cross.UIKitLib;
using ObjCRuntime;

namespace Nivaes.App.Cross.Sample.UIKitLib
{
    [MvxFromStoryboard("Main")]
    [PagePresentation(WrapInNavigationController = false)]
    public partial class Page2View
        : MvxViewController<Page2ViewModel>
    {
        public Page2View(NativeHandle handle) : base(handle)
        {
        }
    }
}
