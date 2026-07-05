using System.Diagnostics.CodeAnalysis;
using Android.Runtime;

namespace Nivaes.App.Cross.Droid.Leanback
{
    [Register("nivaes.cross.leanback.BrowseSupportFragment")]
    [RequiresUnreferencedCode("Bindings require unreferenced code")]
    public class MvxBrowseSupportFragment
        : MvxEventSourceBrowseSupportFragment, IMvxFragmentView
    {
        /// <summary>
        /// Create new instance of a MvxBrowseSupportFragment
        /// </summary>
        /// <param name="bundle">Usually this would be MvxViewModelRequest serialized</param>
        /// <returns>Returns an instance of a MvxFragment</returns>
        public static MvxBrowseSupportFragment NewInstance(Bundle bundle)
        {
            // Setting Arguments needs to happen before Fragment is attached
            // to Activity. Arguments are persisted when Fragment is recreated!
            var fragment = new MvxBrowseSupportFragment { Arguments = bundle };

            return fragment;
        }

        protected MvxBrowseSupportFragment()
        {
            var _ = new MvxBindingFragmentAdapter(this);
        }

        protected MvxBrowseSupportFragment(IntPtr javaReference, JniHandleOwnership transfer)
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
    }

    [RequiresUnreferencedCode("Bindings require unreferenced code")]
    public abstract class MvxBrowseSupportFragment<TViewModel> : MvxBrowseSupportFragment, IMvxFragmentView<TViewModel>
        where TViewModel : class, ICrossViewModel
    {
        protected MvxBrowseSupportFragment()
        {
        }

        protected MvxBrowseSupportFragment(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
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
