namespace Nivaes.App.Cross.Sample.UIKitOS
{
    using System.Diagnostics.CodeAnalysis;
    using Nivaes.App.Cross.UIKitOS;
    using ObjCRuntime;

    [MvxFromStoryboard("Main")]
    [MvxTabPresentation(WrapInNavigationController = true, TabIconName = "home", TabName = "Tab 1")]
    [RequiresUnreferencedCode("Bindings require unreferenced code")]
    public partial class Tab1View
        : MvxViewController<Tab1ViewModel>
    {
        public Tab1View(NativeHandle handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = CreateBindingSet();
            set.Bind(btnModal).To(vm => vm.OpenModalCommand);
            set.Bind(btnNavModal).To(vm => vm.OpenNavModalCommand);
            set.Bind(btnChild).To(vm => vm.OpenChildCommand);
            set.Bind(btnTab2).To(vm => vm.OpenTab2Command);
            set.Apply();
        }
    }
}
