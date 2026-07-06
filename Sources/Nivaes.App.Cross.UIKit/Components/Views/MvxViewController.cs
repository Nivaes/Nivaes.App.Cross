using ObjCRuntime;

namespace Nivaes.App.Cross.UIKitLib
{
    public class MvxViewController<TViewModel>
        : MvxEventSourceViewController, IMvxIosView<TViewModel>
        where TViewModel : ICrossViewModel
    {
        public MvxViewController() : base()
        {
            this.AdaptForBinding();
        }

        public MvxViewController(NSCoder coder) : base(coder)
        {
            this.AdaptForBinding();
        }

        protected MvxViewController(NSObjectFlag t) : base(t)
        {
            this.AdaptForBinding();
        }

        protected internal MvxViewController(NativeHandle handle) : base(handle)
        {
            this.AdaptForBinding();
        }

        public MvxViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
            this.AdaptForBinding();
        }

        #region Data
        public ICrossBindingContext? BindingContext { get; set; }

        public object? DataContext
        {
            get { return BindingContext?.DataContext; }
            set { BindingContext?.DataContext = value; }
        }

        public TViewModel? ViewModel
        {
            get { return (TViewModel?)DataContext; }
            set { DataContext = value; }
        }

        ICrossViewModel? ICrossView.ViewModel { get => ViewModel; set => ViewModel = (TViewModel?)value; }
        #endregion

        public CrossViewModelRequest? Request { get; set; } = default;

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

        public override void DidMoveToParentViewController(UIViewController? parent)
        {
            base.DidMoveToParentViewController(parent);
            if (parent == null)
                ViewModel?.ViewDestroy();
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject? sender)
        {
            base.PrepareForSegue(segue, sender);
            this.ViewModelRequestForSegue(segue, sender);
        }

        public CrossFluentBindingDescriptionSet<IMvxIosView<TViewModel>, TViewModel> CreateBindingSet()
        {
            return this.CreateBindingSet<IMvxIosView<TViewModel>, TViewModel>();
        }
    }
}
