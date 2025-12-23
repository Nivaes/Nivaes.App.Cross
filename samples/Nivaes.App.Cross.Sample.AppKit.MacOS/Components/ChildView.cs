namespace Playground.Mac
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Platforms.Mac.Views;
    using Nivaes.App.Cross.AppKit;
    using ObjCRuntime;
    using Playground.Core.ViewModels;

    [MvxFromStoryboard("Main")]
    [CrossContentPresentation]
    [RequiresUnreferencedCode("MvxBindings require unreferenced code")]
    public partial class ChildView
        : MvxViewController<ChildViewModel>
    {
        public ChildView(NativeHandle handle) : base(handle)
        {
            Title = "Child view";
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = CreateBindingSet();
            set.Bind(btnRoot).To(vm => vm.ShowRootCommand);
            set.Apply();
        }
    }
}
