namespace Nivaes.App.Cross.Sample.UIKitOS
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Platforms.Ios.Views;
    using Nivaes.App.Cross.UIKitOS;
    using ObjCRuntime;

    [MvxFromStoryboard("Main")]
    [MvxTabPresentation(WrapInNavigationController = false)]
    [RequiresUnreferencedCode("MvxBindings require unreferenced code")]
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
