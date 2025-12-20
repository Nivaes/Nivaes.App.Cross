namespace MvvmCross.Platforms.Mac.Views
{
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Platforms.Mac.Views.Base;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;
    using ObjCRuntime;

    public class MvxTabViewController : MvxEventSourceTabViewController, IMvxTabViewController, IMvxMacView
    {
        protected MvxTabViewController()
            : base()
        {
            this.Initialize();
        }

        protected MvxTabViewController(NSCoder coder)
            : base(coder)
        {
            this.Initialize();
        }

        protected MvxTabViewController(NativeHandle handle)
            : base(handle)
        {
            this.Initialize();
        }

        protected MvxTabViewController(NSObjectFlag flag)
            : base(flag)
        {
            this.Initialize();
        }

        // Shared initialization code
        private void Initialize()
        {
            this.AdaptForBinding();
        }

        public void ShowTabView(NSViewController viewController, string tabTitle)
        {
            AddChildViewController(viewController);

            if (!string.IsNullOrEmpty(tabTitle))
                TabViewItems[ChildViewControllers.Count() - 1].Label = tabTitle;
        }

        public bool CloseTabView(ICrossViewModel viewModel)
        {
            var index = ChildViewControllers.Select(v => (MvxViewController)v).ToList().FindIndex(vc => viewModel == vc.ViewModel);

            if (index >= 0)
            {
                RemoveChildViewController(index);
                return true;
            }

            return false;
        }

        public object DataContext
        {
            get { return this.BindingContext.DataContext; }
            set { this.BindingContext.DataContext = value; }
        }

        public ICrossViewModel ViewModel
        {
            get { return (ICrossViewModel)this.DataContext; }
            set { this.DataContext = value; }
        }

        public MvxViewModelRequest Request { get; set; }

        public IMvxBindingContext BindingContext { get; set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ViewModel?.ViewCreated();
        }

        public override void ViewWillAppear()
        {
            base.ViewWillAppear();
            ViewModel?.ViewAppearing();
        }

        public override void ViewDidAppear()
        {
            base.ViewDidAppear();
            ViewModel?.ViewAppeared();
        }

        public override void ViewWillDisappear()
        {
            base.ViewWillDisappear();
            ViewModel?.ViewDisappearing();
        }

        public override void ViewDidDisappear()
        {
            base.ViewDidDisappear();
            ViewModel?.ViewDisappeared();
        }

        public override void PrepareForSegue(NSStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            this.ViewModelRequestForSegue(segue, sender);
        }

        public override void RemoveFromParentViewController()
        {
            base.RemoveFromParentViewController();
            ViewModel?.ViewDestroy();
        }
    }

    public class MvxTabViewController<TViewModel> : MvxTabViewController, IMvxMacView<TViewModel>
        where TViewModel : class, ICrossViewModel
    {
        public MvxTabViewController()
        {
        }

        public MvxTabViewController(NativeHandle handle)
            : base(handle)
        {
        }

        protected MvxTabViewController(NSObjectFlag flag)
            : base(flag)
        {
        }

        public MvxTabViewController(NSCoder coder) : base(coder)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public MvxFluentBindingDescriptionSet<IMvxMacView<TViewModel>, TViewModel> CreateBindingSet()
        {
            return this.CreateBindingSet<IMvxMacView<TViewModel>, TViewModel>();
        }
    }
}
