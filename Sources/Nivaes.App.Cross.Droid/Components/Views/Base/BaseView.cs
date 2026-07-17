using Android.Runtime;
using Android.Views;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace Nivaes.App.Cross.Droid
{
    /// <summary>Base view.</summary>
    public abstract class BaseView<TViewModel>
        : MvxFragment<TViewModel>
        where TViewModel : IBaseViewModel
    {
        protected Toolbar MainToolbar { get; private set; }

        protected abstract int LayoutId { get; }

        /// <summary>If true show the hamburger menu.</summary>
        protected virtual bool ShowHamburgerMenu => false;

        #region Constructors
        protected BaseView()
            : base()
        {
            base.RetainInstance = false;
        }

        protected BaseView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            base.RetainInstance = false;
        }
        #endregion


        public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(LayoutId, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            if (base.Activity is IMainActivity mainActivity)
            {
                MainToolbar = base.Activity.FindViewById<Toolbar>(Resource.Id.main_toolbar);

                if (MainToolbar != null)
                {
                    //mainActivity.SetSupportActionBar(MainToolbar);
                    mainActivity.ShowHamburgerMenu = ShowHamburgerMenu;
                }
            }
        }
    }
}
