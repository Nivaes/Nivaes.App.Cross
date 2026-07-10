using System.Diagnostics.CodeAnalysis;
using Android.Runtime;

namespace Nivaes.App.Cross.Droid.Leanback
{
    [Register("nivaes.cross.leanback.fragments.HeadersSupportFragment")]
    [RequiresUnreferencedCode("Bindings require unreferenced code")]
    public class MvxHeadersSupportFragment<TViewModel>
        : MvxEventSourceHeadersSupportFragment, IMvxFragmentView<TViewModel>, IMvxFragmentView
        where TViewModel : class, ICrossViewModel
    {
        #region Constructors
        protected MvxHeadersSupportFragment()
        {
            var _ = new MvxBindingFragmentAdapter(this);
        }

        protected MvxHeadersSupportFragment(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }
        #endregion

        #region Data
        public ICrossBindingContext? BindingContext { get; set; }

        private object? _dataContext;

        public object? DataContext
        {
            get => _dataContext;
            set
            {
                _dataContext = value;
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

        #region Life cycle
        public virtual void OnViewModelSet()
        {
        }

        public string UniqueImmutableCacheTag => Tag;
        #endregion

        #region Binding
        public CrossFluentBindingDescriptionSet<IMvxFragmentView<TViewModel>, TViewModel> CreateBindingSet()
        {
            return this.CreateBindingSet<IMvxFragmentView<TViewModel>, TViewModel>();
        }
        #endregion
    }
}
