using Android.Views;

namespace Nivaes.App.Cross.Droid
{
    /// <summary>Base master view.</summary>
    public abstract class BaseMasterView<TViewModel>
        : BaseView<TViewModel>
        where TViewModel : IMasterViewModel
    {
        protected virtual int DialogToolbarId { get; } = Resource.Id.main_toolbar;

        protected string Title { get; set; }

        protected override bool ShowHamburgerMenu => true;

        private IMenuItem mSearchAction;
        protected virtual bool ShowSearch => false;
        protected virtual bool ShowSuggestions => false;
        protected virtual int MenuResourceId => 0;

        protected abstract int SearchId { get; } //Resource.Id.search

        protected BaseMasterView()
        {
            //base.RetainInstance = true;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            //if (base.Activity is IMainActivity mainActivity)
            //{
            //    if (MainToolbar != null)
            //    {
            //        mainActivity.SetSupportActionBar(MainToolbar);
            //        mainActivity.ShowHamburgerMenu = ShowHamburgerMenu;

            //        if (!string.IsNullOrEmpty(Title))
            //        {
            //            MainToolbar.Title = Title;
            //        }

            //        mainActivity.StartActionBar();
            //    }
            //}
        }

        //public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        //{
        //    if (menu == null) throw new ArgumentNullException(nameof(menu));
        //    if (inflater == null) throw new ArgumentNullException(nameof(inflater));

        //    if (MenuResourceId > 0)
        //    {
        //        inflater.Inflate(MenuResourceId, menu);
        //    }

        //    mSearchAction = menu.FindItem(SearchId);

        //    if (ShowSearch)
        //    {
        //        CreateSearch();
        //    }
        //}

        private void CreateSearch()
        {
            //if (System.Diagnostics.Debugger.IsAttached)
            System.Diagnostics.Debugger.Break();

            //SearchManager searchManager = (SearchManager)base.Activity.GetSystemService(Android.Content.Context.SearchService);
            //var searchView = (Android.Widget.SearchView)mSearchAction.ActionView;
            //searchView.SetSearchableInfo(searchManager.GetSearchableInfo(base.Activity.ComponentName));

            //searchView.QueryTextSubmit += async (o, e) =>
            //{
            //    await SearchQuery(e.Query).ConfigureAwait(false);
            //    e.Handled = false;
            //};

            //searchView.Close += async (o, e) =>
            //{
            //    await SearchQuery(string.Empty).ConfigureAwait(false);
            //    searchManager.StopSearch();

            //    e.Handled = false;
            //};

            //searchView.QueryTextChange += async (o, e) =>
            //{
            //    if (ShowSuggestions)
            //    {
            //        var suggestions = await SearchSuggestions(e.NewText).ConfigureAwait(false);
            //        searchView.SuggestionsAdapter = new SuggestionsAdapter(base.Context, suggestions);
            //    }
            //    else
            //    {
            //        await SearchQuery(e.NewText).ConfigureAwait(false);
            //    }
            //    e.Handled = false;
            //};

            //searchView.SuggestionClick += (o, e) =>
            //{
            //    searchView.SetQuery(((SuggestionsAdapter)(searchView.SuggestionsAdapter)).GetValue(), true);

            //    e.Handled = false;
            //};
        }

        protected virtual Task SearchQuery(string query)
        {
            return Task.FromResult(0);
        }

        protected virtual Task<IEnumerable<string>> SearchSuggestions(string query)
        {
            return Task.FromResult<IEnumerable<string>>(Array.Empty<string>());
        }
    }
}
