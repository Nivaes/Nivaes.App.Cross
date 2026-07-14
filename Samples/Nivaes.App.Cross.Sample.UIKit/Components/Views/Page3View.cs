using Nivaes.App.Cross.UIKitLib;
using ObjCRuntime;

namespace Nivaes.App.Cross.Sample.UIKitLib
{
    [MvxFromStoryboard("Main")]
    [PagePresentation(WrapInNavigationController = false)]
    public partial class Page3View : MvxViewController<Page3ViewModel>
    {
        public Page3View(NativeHandle handle) : base(handle)
        {
        }
    }
}
