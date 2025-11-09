namespace Nivaes.App.Cross.UIKit
{
    using ObjCRuntime;

    public class CrossViewController<TViewModel> :
        CrossEventSourceViewController, 
        ICrossIosView<TViewModel>
        where TViewModel : class, ICrossViewModel
    {
        public CrossViewController() : base()
        {
            this.AdaptForBinding();
        }

        public CrossViewController(NSCoder coder) : base(coder)
        {
            this.AdaptForBinding();
        }

        protected CrossViewController(NSObjectFlag t) : base(t)
        {
            this.AdaptForBinding();
        }

        protected internal CrossViewController(NativeHandle handle) : base(handle)
        {
            this.AdaptForBinding();
        }

        public CrossViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
            this.AdaptForBinding();
        }

        public object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }

        public TViewModel ViewModel
        {
            get { return (TViewModel)DataContext; }
            set { DataContext = value; }
        }

        public CrossViewModelRequest<TViewModel> Request { get; set; }

        public ICrossBindingContext BindingContext { get; set; }
        ICrossViewModelRequest ICrossIosView.Request { get => Request; set => throw new NotImplementedException(); }
        ICrossViewModel? ICrossView.ViewModel { get => ViewModel; set => throw new NotImplementedException(); }

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

        public CrossFluentBindingDescriptionSet<ICrossIosView<TViewModel>, TViewModel> CreateBindingSet()
        {
            return this.CreateBindingSet<ICrossIosView<TViewModel>, TViewModel>();
        }
    }
}
