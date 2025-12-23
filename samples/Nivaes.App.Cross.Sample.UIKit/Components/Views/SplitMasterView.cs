namespace Playground.iOS.Views
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Platforms.Ios.Views;
    using Nivaes.App.Cross.UIKit;
    using ObjCRuntime;
    using Playground.Core.ViewModels;

    [MvxFromStoryboard("Main")]
    [MvxSplitViewPresentation(MasterDetailPosition.Master)]
    [RequiresUnreferencedCode("MvxBindings require unreferenced code")]
    public partial class SplitMasterView : MvxViewController<SplitMasterViewModel>
    {
        public SplitMasterView(NativeHandle handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = CreateBindingSet();
            set.Bind(btnDetail).To(vm => vm.OpenDetailCommand);
            set.Bind(btnDetailNav).To(vm => vm.OpenDetailNavCommand);
            set.Bind(btnStack).To(vm => vm.ShowRootViewModel);
            set.Apply();
        }
    }
}
