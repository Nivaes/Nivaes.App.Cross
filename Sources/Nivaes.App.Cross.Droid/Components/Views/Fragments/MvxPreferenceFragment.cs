using System;
using System.Diagnostics.CodeAnalysis;
using Android.OS;
using Android.Runtime;
using Nivaes.App.Cross;

namespace Nivaes.App.Cross.Droid
{
    [Register("nivaes.cross.PreferenceFragment")]
    public abstract class MvxPreferenceFragment : MvxEventSourcePreferenceFragment, IMvxFragmentView
    {
        [RequiresUnreferencedCode("This constructor uses reflection which may not be preserved during trimming.")]
        protected MvxPreferenceFragment()
        {
            this.AddEventListeners();
        }

        protected MvxPreferenceFragment(IntPtr javaReference, JniHandleOwnership transfer)
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

    public abstract class MvxPreferenceFragment<TViewModel>
        : MvxPreferenceFragment, IMvxFragmentView<TViewModel>
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

        public CrossFluentBindingDescriptionSet<IMvxFragmentView<TViewModel>, TViewModel> CreateBindingSet()
        {
            return this.CreateBindingSet<IMvxFragmentView<TViewModel>, TViewModel>();
        }
    }
}
