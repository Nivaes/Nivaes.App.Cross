using Android.Runtime;
using Android.Views;
using SearchView = AndroidX.AppCompat.Widget.SearchView;

namespace Nivaes.App.Cross.Droid
{
    public abstract class BaseSelectorDialogView<TViewModel>
        : BaseDialogView<TViewModel>
        where TViewModel : ISelectorDialogViewModel
    {
        protected virtual bool ShowSearch => false;
        protected virtual bool ShowSuggestions => false;
        protected override int MenuResourceId => ShowSearch ? Resource.Menu.default_selector_dialog_menu : 0;

        #region Constructors
        protected BaseSelectorDialogView()
            : base()
        { }

        protected BaseSelectorDialogView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }
        #endregion

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);

            if (base.DialogToolbar != null)
            {
                if (ShowSearch)
                {
                    CreateSearch();
                }
            }
        }

        private void CreateSearch()
        {
            SearchManager searchManager = (SearchManager)base.Activity.GetSystemService(Android.Content.Context.SearchService);
            var dialogMenu = base.DialogToolbar.Menu;

            var searchAction = dialogMenu.FindItem(Resource.Id.action_search);
            var searchView = (SearchView)searchAction.ActionView;
            searchView.SetSearchableInfo(searchManager.GetSearchableInfo(base.Activity.ComponentName));

            searchView.QueryTextSubmit += async (o, e) =>
            {
                await SearchQuery(e.NewText).ConfigureAwait(false);
                e.Handled = false;
            };

            searchView.Close += async (o, e) =>
            {
                await SearchQuery(string.Empty).ConfigureAwait(false);
                searchManager.StopSearch();
            };

            searchView.QueryTextChange += async (o, e) =>
            {
                if (ShowSuggestions)
                {
                    var suggestions = await SearchSuggestions(e.NewText).ConfigureAwait(false);
                    searchView.SuggestionsAdapter = new SuggestionsAdapter(base.Context, suggestions);
                }
                else
                {
                    await SearchQuery(e.NewText).ConfigureAwait(false);
                }
                e.Handled = false;
            };


            searchView.SuggestionClick += (o, e) =>
            {
                searchView.SetQuery(((SuggestionsAdapter)(searchView.SuggestionsAdapter)).GetValue(), true);
            };
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
