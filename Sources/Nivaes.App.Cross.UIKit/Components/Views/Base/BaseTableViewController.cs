namespace Nivaes.App.Cross.UIKitLib
{
    /// <summary> A base table view controller </summary>
    public abstract class BaseTableViewController<TViewModel>
        : MvxTableViewController<TViewModel>, IUISearchResultsUpdating
        where TViewModel : IBaseViewModel
    {
        protected virtual bool IsShowSearchBar => false;
        protected virtual string SearchPlaceholder => string.Empty;

        #region Constructors
        public BaseTableViewController()
            : base()
        {
        }

        protected BaseTableViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.Title = ViewModel.Title;

            base.ViewDidLoad();

            base.TableView.TableFooterView = new UIView();

            if (IsShowSearchBar)
            {
                ShowSearchBar();
            }
        }
        #endregion

        #region Search
        private void ShowSearchBar()
        {
            var searchController = new UISearchController(searchResultsController: null)
            {
                HidesNavigationBarDuringPresentation = true,
                ObscuresBackgroundDuringPresentation = false,
#if IOS || MACCATALYST
                DimsBackgroundDuringPresentation = false,
#endif
                SearchResultsUpdater = this
            };

            searchController.SearchBar.SizeToFit();
            searchController.SearchBar.SearchBarStyle = UISearchBarStyle.Prominent;

            var textField = (UITextField)searchController.SearchBar.ValueForKey((NSString)"_searchField");
            textField.TextColor = UISearchBar.Appearance.TintColor;

            base.DefinesPresentationContext = true;

            if (!string.IsNullOrEmpty(SearchPlaceholder))
                searchController.SearchBar.Placeholder = SearchPlaceholder;

#if IOS || MACCATALYST
            base.NavigationItem.SearchController = searchController;
#endif
        }

        void IUISearchResultsUpdating.UpdateSearchResultsForSearchController(UISearchController searchController)
        {
            UpdateSearchResultsForSearchController(searchController);
        }

        protected virtual void UpdateSearchResultsForSearchController(UISearchController searchController)
        {
        }
        #endregion
    }
}
