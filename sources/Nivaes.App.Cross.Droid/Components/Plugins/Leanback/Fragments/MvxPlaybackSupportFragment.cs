namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Android.OS;
    using Android.Runtime;
    using Nivaes.App.Cross;

    [Register("mvvmcross.droidx.leanback.fragments.MvxPlaybackSupportFragment")]
    [RequiresUnreferencedCode("MvxBindings require unreferenced code")]
    public class MvxPlaybackSupportFragment
        : MvxEventSourcePlaybackSupportFragment, IMvxFragmentView
    {
        /// <summary>
        /// Create new instance of a MvxSearchSupportFragment
        /// </summary>
        /// <param name="bundle">Usually this would be MvxViewModelRequest serialized</param>
        /// <returns>Returns an instance of a MvxFragment</returns>
        public static MvxPlaybackSupportFragment NewInstance(Bundle bundle)
        {
            // Setting Arguments needs to happen before Fragment is attached
            // to Activity. Arguments are persisted when Fragment is recreated!
            var fragment = new MvxPlaybackSupportFragment { Arguments = bundle };

            return fragment;
        }

        protected MvxPlaybackSupportFragment()
        {
            var _ = new MvxBindingFragmentAdapter(this);
        }

        protected MvxPlaybackSupportFragment(IntPtr javaReference, JniHandleOwnership transfer)
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

    [RequiresUnreferencedCode("MvxBindings require unreferenced code")]
    public abstract class MvxPlaybackSupportFragment<TViewModel> : MvxPlaybackSupportFragment, IMvxFragmentView<TViewModel>
        where TViewModel : class, ICrossViewModel
    {
        protected MvxPlaybackSupportFragment()
        {
        }

        protected MvxPlaybackSupportFragment(IntPtr javaReference, JniHandleOwnership transfer)
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
