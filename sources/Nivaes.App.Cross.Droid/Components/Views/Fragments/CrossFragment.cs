namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Android.Runtime;

    [Register("mvvmcross.platforms.android.views.fragments.MvxFragment")]
    public class CrossFragment
        : CrossEventSourceFragment
        , ICrossFragmentView
    {
        /// <summary>
        /// Create new instance of a Fragment
        /// </summary>
        /// <param name="bundle">Usually this would be CrossViewModelRequest serialized</param>
        /// <returns>Returns an instance of a MvxFragment</returns>
        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
        public static CrossFragment NewInstance(Bundle bundle)
        {
            // Setting Arguments needs to happen before Fragment is attached
            // to Activity. Arguments are persisted when Fragment is recreated!
            var fragment = new CrossFragment { Arguments = bundle };

            return fragment;
        }

        protected CrossFragment(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        [RequiresUnreferencedCode("This constructor uses reflection which may not be preserved during trimming.")]
        protected CrossFragment()
        {
            this.AddEventListeners();
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

    public abstract class MvxFragment<TViewModel> : CrossFragment, ICrossFragmentView<TViewModel>
        where TViewModel : class, ICrossViewModel
    {
        [RequiresUnreferencedCode("This constructor uses reflection which may not be preserved during trimming.")]
        protected MvxFragment()
        {
        }

        protected MvxFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public CrossFluentBindingDescriptionSet<IMvxFragmentView<TViewModel>, TViewModel> CreateBindingSet()
        {
            return this.CreateBindingSet<IMvxFragmentView<TViewModel>, TViewModel>();
        }
    }
}
