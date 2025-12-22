namespace Playground.Mac
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using AppKit;
    using MvvmCross.Platforms.Mac.Presenters.Attributes;
    using MvvmCross.Platforms.Mac.Views;
    using MvvmCross.Presenters;
    using MvvmCross.Presenters.Attributes;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;
    using ObjCRuntime;
    using Playground.Core.ViewModels;

    [MvxFromStoryboard("Main")]
    [MvxWindowPresentation(PositionX = 300)]
    [RequiresUnreferencedCode("MvxBindings require unreferenced code")]
    public partial class RootView : MvxViewController<RootViewModel>, ICrossOverridePresentationAttribute
    {
        // prevents presentation in a new window when navigating back to root from a child
        private static bool WasPresentedInWindow = false;

        public bool MyValue { get; set; } = true;

        public RootView(NativeHandle handle) : base(handle)
        {
            Title = "Root view";
        }

        public override void LoadView()
        {
            base.LoadView();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = CreateBindingSet();
            set.Bind(btnChild).To(vm => vm.ShowChildCommand);
            set.Bind(btnModal).To(vm => vm.ShowModalCommand);
            set.Bind(btnSheet).To(vm => vm.ShowSheetCommand);
            set.Bind(btnWindow).To(vm => vm.ShowWindowCommand);
            set.Bind(btnTabs).To(vm => vm.ShowTabsCommand);
            set.Apply();
        }

        public override void ViewDidDisappear()
        {
            base.ViewDidDisappear();
        }

        public MvxBasePresentationAttribute PresentationAttribute(CrossViewModelRequest request)
        {
            if (!WasPresentedInWindow)
            {
                WasPresentedInWindow = true;
                return null;
            }

            return new MvxContentPresentationAttribute
            {
                WindowIdentifier = typeof(RootView).Name
            };
        }
    }
}
