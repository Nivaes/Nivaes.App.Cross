using Android.Runtime;
using Android.Views;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace Nivaes.App.Cross.Droid
{
    /// <summary>Base detail view.</summary>
    public abstract class BaseDetailView<TViewModel>
        : BaseView<TViewModel>
        where TViewModel : IBaseDetailViewModel
    {
        #region Proporties
        protected override bool ShowHamburgerMenu => false;

        protected string Title { get; set; }

        protected virtual int MenuResourceId { get; } = 0;

        protected Toolbar DetailToolbar { get; private set; }
        #endregion

        #region Constructors
        protected BaseDetailView()
            : base()
        {
            //base.RetainInstance = false;
        }

        protected BaseDetailView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            //base.RetainInstance = false;
        }
        #endregion

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));

            base.OnViewCreated(view, savedInstanceState);

            //if (System.Diagnostics.Debugger.IsAttached)
            System.Diagnostics.Debugger.Break();

            //if (base.Activity is IMainActivity mainActivity)
            //{
            //    DetailToolbar = view.FindViewById<Toolbar>(Resource.Id.detail_toolbar);

            //    if (DetailToolbar != null)
            //    {
            //        if (MainToolbar == null)
            //        {
            //            mainActivity.SetSupportActionBar(DetailToolbar);
            //        }

            //        mainActivity.ShowHamburgerMenu = ShowHamburgerMenu;

            //        if (!string.IsNullOrEmpty(Title))
            //        {
            //            DetailToolbar.Title = Title;
            //        }

            //        mainActivity.StartActionBar();

            //        if (MenuResourceId != 0)
            //        {
            //            DetailToolbar.InflateMenu(MenuResourceId);

            //            DetailToolbar.MenuItemClick += (o, e) =>
            //            {
            //                OnOptionsItemSelected(e.Item);
            //            };

            //            if (DetailToolbar.Menu is MenuBuilder menuBuilder)
            //                menuBuilder.SetOptionalIconsVisible(true);
            //        }
            //    }
            //}
        }

        //public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        //{
        //    if (inflater == null) throw new ArgumentNullException(nameof(inflater));

        //    if (base.MainToolbar == null && MenuResourceId != 0)
        //    {
        //        inflater.Inflate(MenuResourceId, menu);
        //        base.SetMenuVisibility(true);

        //        if (menu is MenuBuilder menuBuilder)
        //            menuBuilder.SetOptionalIconsVisible(true);
        //    }
        //    base.OnCreateOptionsMenu(menu, inflater);
        //}
    }
}
