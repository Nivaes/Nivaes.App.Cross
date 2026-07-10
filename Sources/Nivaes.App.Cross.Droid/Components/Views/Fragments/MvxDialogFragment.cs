using System.Diagnostics.CodeAnalysis;
using Android.Runtime;

namespace Nivaes.App.Cross.Droid
{
    [Register("nivaes.cross.DialogFragment")]
    public abstract class MvxDialogFragment<TViewModel>
        : MvxEventSourceDialogFragment, IMvxFragmentView<TViewModel>, IMvxFragmentView
        where TViewModel : ICrossViewModel
    {
        protected MvxDialogFragment(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        [RequiresUnreferencedCode("This constructor uses reflection which may not be preserved during trimming.")]
        protected MvxDialogFragment()
        {
            this.AddEventListeners();
        }

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

        ICrossViewModel? ICrossView.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (TViewModel?)value;
        }

        public virtual void OnViewModelSet()
        {
        }

        public string? UniqueImmutableCacheTag => Tag;
        #endregion



        public override void OnCreate(Bundle? savedInstanceState)
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
