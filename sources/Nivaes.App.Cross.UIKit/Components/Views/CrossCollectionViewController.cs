namespace Nivaes.App.Cross.UIKit
{
    using ObjCRuntime;

    public class CrossCollectionViewController
        : CrossEventSourceCollectionViewController, ICrossIosView
    {
        public CrossCollectionViewController()
        {
            this.AdaptForBinding();
        }

        public CrossCollectionViewController(NSCoder coder) : base(coder)
        {
            this.AdaptForBinding();
        }

        protected CrossCollectionViewController(NSObjectFlag t) : base(t)
        {
            this.AdaptForBinding();
        }

        protected internal CrossCollectionViewController(NativeHandle handle) : base(handle)
        {
            this.AdaptForBinding();
        }

        public CrossCollectionViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
            this.AdaptForBinding();
        }

        public CrossCollectionViewController(UICollectionViewLayout layout) : base(layout)
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

        public ICrossBindingContext BindingContext { get; set; }

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

    public class MvxCollectionViewController<TViewModel> : CrossCollectionViewController, ICrossIosView<TViewModel>
        where TViewModel : class, ICrossViewModel
    {
        public MvxCollectionViewController()
        {
        }

        public MvxCollectionViewController(NSCoder coder) : base(coder)
        {
        }

        public MvxCollectionViewController(UICollectionViewLayout layout) : base(layout)
        {
        }

        public MvxCollectionViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        protected MvxCollectionViewController(NSObjectFlag t) : base(t)
        {
        }

        protected internal MvxCollectionViewController(NativeHandle handle) : base(handle)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public CrossFluentBindingDescriptionSet<IMvxIosView<TViewModel>, TViewModel> CreateBindingSet()
        {
            return this.CreateBindingSet<IMvxIosView<TViewModel>, TViewModel>();
        }
    }
}
