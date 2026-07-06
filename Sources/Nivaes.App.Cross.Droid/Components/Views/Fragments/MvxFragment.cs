using Android.Runtime;
using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross.Droid
{
    [Register("nivaes.cross.Fragment")]
    public class MvxFragment<TViewModel>
        : MvxEventSourceFragment, IMvxFragmentView<TViewModel>
        , IMvxFragmentView
        where TViewModel : ICrossViewModel
    {
        /// <summary>
        /// Create new instance of a Fragment
        /// </summary>
        /// <param name="bundle">Usually this would be MvxViewModelRequest serialized</param>
        /// <returns>Returns an instance of a MvxFragment</returns>
        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
        public static MvxFragment NewInstance(Bundle bundle)
        {
            // Setting Arguments needs to happen before Fragment is attached
            // to Activity. Arguments are persisted when Fragment is recreated!
            var fragment = new MvxFragment { Arguments = bundle };

            return fragment;
        }

        protected MvxFragment(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        [RequiresUnreferencedCode("This constructor uses reflection which may not be preserved during trimming.")]
        protected MvxFragment()
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

        public TViewModel ViewModel
        {
            get { return (ICrossViewModel)DataContext; }
            set {
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

        public CrossFluentBindingDescriptionSet<IMvxFragmentView<TViewModel>, TViewModel> CreateBindingSet()
        {
            return this.CreateBindingSet<IMvxFragmentView<TViewModel>, TViewModel>();
        }
    }
}
