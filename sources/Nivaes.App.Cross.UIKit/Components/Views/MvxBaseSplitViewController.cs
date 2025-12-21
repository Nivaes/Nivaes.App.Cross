namespace MvvmCross.Platforms.Ios.Views
{
    using System;
    using Foundation;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Platforms.Ios.Views.Base;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;
    using ObjCRuntime;
    using UIKit;

    public class MvxBaseSplitViewController : MvxEventSourceSplitViewController, IMvxIosView
    {
        public MvxBaseSplitViewController() : base()
        {
            this.AdaptForBinding();
        }

        public MvxBaseSplitViewController(NSCoder coder) : base(coder)
        {
            this.AdaptForBinding();
        }

        protected MvxBaseSplitViewController(NSObjectFlag t) : base(t)
        {
            this.AdaptForBinding();
        }

        protected internal MvxBaseSplitViewController(NativeHandle handle) : base(handle)
        {
            this.AdaptForBinding();
        }

        public MvxBaseSplitViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
            this.AdaptForBinding();
        }

        public MvxBaseSplitViewController(UISplitViewControllerStyle style) : base(style)
        {
            this.AdaptForBinding();
        }

        public object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }

        public ICrossViewModel ViewModel
        {
            get { return DataContext as ICrossViewModel; }
            set { DataContext = value; }
        }

        public CrossViewModelRequest Request { get; set; }

        public IMvxBindingContext BindingContext { get; set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ViewModel?.ViewCreated();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ViewModel?.ViewAppearing();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            ViewModel?.ViewAppeared();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            ViewModel?.ViewDisappearing();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            ViewModel?.ViewDisappeared();
        }

        public override void DidMoveToParentViewController(UIViewController parent)
        {
            base.DidMoveToParentViewController(parent);
            if (parent == null)
                ViewModel?.ViewDestroy();
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            this.ViewModelRequestForSegue(segue, sender);
        }
    }

    public class MvxBaseSplitViewController<TViewModel> : MvxBaseSplitViewController, IMvxIosView<TViewModel>
        where TViewModel : class, ICrossViewModel
    {
        public MvxBaseSplitViewController()
        {
        }

        public MvxBaseSplitViewController(NSCoder coder) : base(coder)
        {
        }

        public MvxBaseSplitViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        protected MvxBaseSplitViewController(NSObjectFlag t) : base(t)
        {
        }

        protected internal MvxBaseSplitViewController(NativeHandle handle) : base(handle)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public MvxFluentBindingDescriptionSet<IMvxIosView<TViewModel>, TViewModel> CreateBindingSet()
        {
            return this.CreateBindingSet<IMvxIosView<TViewModel>, TViewModel>();
        }
    }
}
