using Android.Content;
using Android.Runtime;

namespace Nivaes.App.Cross.Droid;

public abstract class CrossActivity<TViewModel>
    : CrossEventSourceActivity, ICrossActivity, IMvxAndroidView<TViewModel>
    where TViewModel : ICrossViewModel
{
    protected CrossActivity(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
    }

    protected CrossActivity()
    {
        BindingContext = new MvxAndroidBindingContext(this, this);
        this.AddEventListeners();
    }

    #region Data
    public ICrossBindingContext? BindingContext { get; set; }

    public object? DataContext
    {
        get => BindingContext?.DataContext;
        set
        {
            if (BindingContext != null)
                BindingContext.DataContext = value;
        }
    }

    public TViewModel? ViewModel
    {
        get => (TViewModel?)DataContext;
        set
        {
            DataContext = value;
            OnViewModelSet();
        }
    }

    ICrossViewModel? ICrossView.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (TViewModel?)value;
    }
    #endregion

    public void MvxInternalStartActivityForResult(Intent intent, int requestCode)
    {
        StartActivityForResult(intent, requestCode);
    }

    // ReSharper disable once InconsistentNaming
    public override void SetContentView(int layoutResID)
    {
        if (BaseContextToAttach(this) is CrossContextWrapper)
        {
            var view = this.BindingInflate(layoutResID, null);
            SetContentView(view);
            return;
        }

        base.SetContentView(layoutResID);
    }

    protected virtual void OnViewModelSet()
    {
    }

    protected virtual Context BaseContextToAttach(Context? @base)
        => CrossContextWrapper.Wrap(@base, this);

    protected override void AttachBaseContext(Context? @base)
    {
        if (this is ICrossSetupMonitor)
        {
            // Do not attach our inflater to splash screens.
            base.AttachBaseContext(@base);
            return;
        }
        base.AttachBaseContext(BaseContextToAttach(@base));
    }

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        ViewModel?.ViewCreated();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        ViewModel?.ViewDestroy(IsFinishing);
    }

    protected override void OnStart()
    {
        base.OnStart();
        ViewModel?.ViewAppearing();
    }

    protected override void OnResume()
    {
        base.OnResume();
        ViewModel?.ViewAppeared();
    }

    protected override void OnPause()
    {
        base.OnPause();
        ViewModel?.ViewDisappearing();
    }

    protected override void OnStop()
    {
        base.OnStop();
        ViewModel?.ViewDisappeared();
    }

    public CrossFluentBindingDescriptionSet<IMvxAndroidView<TViewModel>, TViewModel> CreateBindingSet()
        => this.CreateBindingSet<IMvxAndroidView<TViewModel>, TViewModel>();
}