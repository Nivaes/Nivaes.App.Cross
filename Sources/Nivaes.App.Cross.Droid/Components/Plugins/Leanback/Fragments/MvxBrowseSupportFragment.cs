using System.Diagnostics.CodeAnalysis;
using Android.Runtime;
using AndroidX.Lifecycle;

namespace Nivaes.App.Cross.Droid.Leanback
{
    //[Register("nivaes.cross.leanback.fragments.BrowseSupportFragment")]
    public abstract class MvxBrowseSupportFragment<TViewModel>
        : MvxEventSourceBrowseSupportFragment, IMvxFragmentView<TViewModel>
        where TViewModel : class, ICrossViewModel
    {
        protected MvxBrowseSupportFragment()
        {
            var _ = new MvxBindingFragmentAdapter(this);
        }

        protected MvxBrowseSupportFragment(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public ICrossBindingContext? BindingContext { get; set; }

        private object? _dataContext;

        public object? DataContext
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

        public TViewModel? ViewModel
        {
            get
            {
                return (TViewModel?)DataContext;
            }
            set
            {
                DataContext = value;
                OnViewModelSet();
            }
        }

        ICrossViewModel? ICrossView.ViewModel { get => ViewModel; set => ViewModel = (TViewModel?)value; }

        public virtual void OnViewModelSet()
        {
        }

        public string UniqueImmutableCacheTag => Tag;        

        public CrossFluentBindingDescriptionSet<IMvxFragmentView<TViewModel>, TViewModel> CreateBindingSet()
        {
            return this.CreateBindingSet<IMvxFragmentView<TViewModel>, TViewModel>();
        }
    }
}
