using System.Collections;

namespace Nivaes.App.Cross.UIKitLib
{
    public class LoadDataSimpleTableViewSource
        : MvxSimpleTableViewSource
    {
        #region Properties
        public override IEnumerable? ItemsSource
        {
            get => base.ItemsSource;
            set
            {
                base.ItemsSource = value;

                LoadDatas(value);
            }
        }
        #endregion

        #region Constructors
        public LoadDataSimpleTableViewSource(IntPtr handle)
            : base(handle)
        { }

        public LoadDataSimpleTableViewSource(UITableView tableView, string nibName, string? cellIdentifier = null,
                                        NSBundle? bundle = null, bool registerNibForCellReuse = true)
          : base(tableView, nibName, cellIdentifier, bundle, registerNibForCellReuse)
        { }

        public LoadDataSimpleTableViewSource(UITableView tableView, Type cellType, string? cellIdentifier = null)
            : base(tableView, cellType, cellIdentifier)
        { }
        #endregion

        #region Scrolled
        public override async void Scrolled(UIScrollView scrollView)
        {
            if (scrollView is UITableView tableView)
            {
                if (tableView.Source is LoadDataSimpleTableViewSource tableViewSource)
                {
                    if (tableViewSource.ItemsSource is ILoadingDataObservableCollection dataObservableCollection)
                    {
                        var last = tableView.IndexPathsForVisibleRows!.LastOrDefault();
                        if (last != null && last.Item >= dataObservableCollection.CountMargin)
                        {
                            await dataObservableCollection.LoadDatas().ConfigureAwait(false);
                        }
                    }
                }
            }
        }
        #endregion

        #region Data
        private async void LoadDatas(IEnumerable? itemsSource)
        {
            if (itemsSource is ILoadingDataObservableCollection dataObservableCollection)
            {
                await dataObservableCollection.LoadDatas().ConfigureAwait(false);
            }
        }
        #endregion
    }
}
