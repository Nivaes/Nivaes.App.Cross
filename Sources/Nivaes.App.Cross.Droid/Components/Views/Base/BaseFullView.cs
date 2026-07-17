using Android.Views;

namespace Nivaes.App.Cross.Droid
{
    /// <summary>Base full view.</summary>
    public abstract class BaseFullView<TViewModel>
        : BaseView<TViewModel> // ToDo: Debe eredar de BaseMasterView
        where TViewModel : FullViewModel
    {
        protected virtual int DialogToolbarId { get; } = Resource.Id.main_toolbar;

        protected string Title { get; set; }

        protected override bool ShowHamburgerMenu => true;

        protected BaseFullView()
        {
            base.RetainInstance = true;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            if (base.Activity is IMainActivity mainActivity)
            {
                if (MainToolbar != null)
                {
                    mainActivity.SetSupportActionBar(MainToolbar);
                    mainActivity.ShowHamburgerMenu = ShowHamburgerMenu;

                    if (!string.IsNullOrEmpty(Title))
                    {
                        MainToolbar.Title = Title;
                    }

                    mainActivity.StartActionBar();
                }
            }
        }
    }
}
