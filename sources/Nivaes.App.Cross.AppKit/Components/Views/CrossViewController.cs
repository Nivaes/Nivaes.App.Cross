using ObjCRuntime;

namespace Nivaes.App.Cross.AppKitOS;

public class CrossViewController
    : MvxEventSourceViewController
        , IMvxMacView
{
    // Called when created from unmanaged code
    public CrossViewController(NativeHandle handle) : base(handle)
    {
        Initialize();
    }

    // Called when created directly from a XIB file
    public CrossViewController(NSCoder coder) : base(coder)
    {
        Initialize();
    }

    // Call to load from the XIB/NIB file
    public CrossViewController(string viewName, NSBundle bundle) : base(viewName, bundle)
    {
        Initialize();
    }

    // Call to load from the XIB/NIB file
    public CrossViewController(string viewName) : base(viewName, NSBundle.MainBundle)
    {
        Initialize();
    }

    public CrossViewController() : base()
    {
        Initialize();
    }

    // Shared initialization code
    private void Initialize()
    {
        this.AdaptForBinding();
    }

    public object? DataContext
    {
        get { return BindingContext?.DataContext; }
        set { BindingContext?.DataContext = value; }
    }

    public ICrossViewModel? ViewModel
    {
        get { return (ICrossViewModel?)DataContext; }
        set { DataContext = value; }
    }

    public CrossViewModelRequest? Request { get; set; }

    public ICrossBindingContext? BindingContext { get; set; }

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

public class CrossViewController<TViewModel> : CrossViewController, IMvxMacView<TViewModel>
    where TViewModel : class, ICrossViewModel
{
    public CrossViewController()
    {
    }

    public CrossViewController(NativeHandle handle)
        : base(handle)
    {
    }

    protected CrossViewController(string nibName, NSBundle bundle)
        : base(nibName, bundle)
    {
    }

    public CrossViewController(NSCoder coder) : base(coder)
    {
    }

    public new TViewModel ViewModel
    {
        get { return (TViewModel)base.ViewModel; }
        set { base.ViewModel = value; }
    }

    public CrossFluentBindingDescriptionSet<IMvxMacView<TViewModel>, TViewModel> CreateBindingSet()
    {
        return this.CreateBindingSet<IMvxMacView<TViewModel>, TViewModel>();
    }
}
