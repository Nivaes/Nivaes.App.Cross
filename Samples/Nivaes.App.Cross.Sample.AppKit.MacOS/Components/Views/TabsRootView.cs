using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.AppKitLib;
using ObjCRuntime;

namespace Nivaes.App.Cross.Sample.AppKitOS.MacOS
{
    [MvxWindowPresentation(PositionX = 150)]
    [RequiresUnreferencedCode("Bindings require unreferenced code")]
    public partial class TabsRootView
        : MvxTabViewController<TabsRootViewModel>
    {
        private bool _firstTime = true;

        public TabsRootView(NativeHandle handle) : base(handle)
        {
        }

        public override void LoadView()
        {
            base.LoadView();

            View = View ?? new AppKit.NSView();
        }

        public override void ViewWillAppear()
        {
            base.ViewWillAppear();

            if (_firstTime)
            {
                ViewModel.ShowInitialViewModelsCommand.Execute(null);
                _firstTime = false;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override void ViewDidAppear()
        {
            base.ViewDidAppear();

            var set = CreateBindingSet();
            set.Bind(this).For(v => v.SelectedTabViewItemIndex).To(vm => vm.ItemIndex);
            set.Apply();
        }
    }
}
