using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.UIKitLib;
using ObjCRuntime;

namespace Nivaes.App.Cross.Sample.UIKitLib
{
    [MvxFromStoryboard("Main")]
    [TabPresentation(WrapInNavigationController = false)]
    [RequiresUnreferencedCode("Bindings require unreferenced code")]
    public partial class Tab3View : MvxViewController<Tab3ViewModel>, IMvxTabBarItemViewController
    {
        public Tab3View(NativeHandle handle) : base(handle)
        {
        }

        public string TabName => "Third";
        public string TabIconName => "settings";

        public string TabSelectedIconName => "settings";

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = CreateBindingSet();
            set.Bind(btnShowStack).To(vm => vm.ShowRootViewModelCommand);
            set.Bind(btnClose).To(vm => vm.CloseViewModelCommand);
            set.Apply();
        }
    }
}
