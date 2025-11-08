namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Android.Runtime;

    [Register("mvvmcross.platforms.android.views.fragments.MvxPreferenceFragment")]
    public abstract class CrossPreferenceFragment : CrossEventSourcePreferenceFragment, ICrossFragmentView
    {
        [RequiresUnreferencedCode("This constructor uses reflection which may not be preserved during trimming.")]
        protected CrossPreferenceFragment()
        {
            this.AddEventListeners();
        }

        protected CrossPreferenceFragment(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public ICrossBindingContext BindingContext { get; set; }

        private object _dataContext;

        public object DataContext
        {
            get
            {
                return _dataContext;
            }
            set
            {
                _dataContext = value;
                if (BindingContext != null)
                    BindingContext.DataContext = value;
            }
        }

        public virtual ICrossViewModel ViewModel
        {
            get
            {
                return DataContext as ICrossViewModel;
            }
            set
            {
                DataContext = value;
                OnViewModelSet();
            }
        }

        public virtual void OnViewModelSet()
        {
        }

        public string UniqueImmutableCacheTag => Tag;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ViewModel?.ViewCreated();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            ViewModel?.ViewDestroy(viewFinishing: IsRemoving || Activity == null || Activity.IsFinishing);
        }

        public override void OnStart()
        {
            base.OnStart();
            ViewModel?.ViewAppearing();
        }

        public override void OnResume()
        {
            base.OnResume();
            ViewModel?.ViewAppeared();
        }

        public override void OnPause()
        {
            base.OnPause();
            ViewModel?.ViewDisappearing();
        }

        public override void OnStop()
        {
            base.OnStop();
            ViewModel?.ViewDisappeared();
        }
    }

    public abstract class MvxPreferenceFragment<TViewModel> : CrossPreferenceFragment, IMvxFragmentView<TViewModel>
        where TViewModel : class, ICrossViewModel
    {
        [RequiresUnreferencedCode("This constructor uses reflection which may not be preserved during trimming.")]
        protected MvxPreferenceFragment()
        {
        }

        protected MvxPreferenceFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public CrossFluentBindingDescriptionSet<ICrossFragmentView<TViewModel>, TViewModel> CreateBindingSet()
        {
            return this.CreateBindingSet<ICrossFragmentView<TViewModel>, TViewModel>();
        }
    }
}
