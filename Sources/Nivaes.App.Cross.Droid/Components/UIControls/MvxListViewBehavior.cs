using System.Reflection;

namespace Nivaes.App.Cross.Droid
{
    public static class MvxListViewBehavior
    {
        #region LoadData
        public static async void LoadData(this MvxListView listView, object collection)
        {
            if (listView == null) throw new ArgumentNullException(nameof(listView));

            if (collection is ILoadingDataObservableCollection dataObservableCollection)
            {
                await dataObservableCollection.LoadDatas().ConfigureAwait(false);
            }

            listView.Scroll += ListviewScroll;
            _ = new ListViewScrollChangedEventSubscription(listView, ListviewScroll);
        }

        private static async void ListviewScroll(object sender, AbsListView.ScrollEventArgs e)
        {
            if (sender is MvxListView listView)
            {
                if (listView.ItemsSource is ILoadingDataObservableCollection dataObservableCollection)
                {
                    if (e.FirstVisibleItem + e.VisibleItemCount >= dataObservableCollection.CountMargin)
                    {
                        await dataObservableCollection.LoadDatas().ConfigureAwait(false);
                    }
                }
            }
        }

        private class ListViewScrollChangedEventSubscription
            : CrossWeakEventSubscription<MvxListView, AbsListView.ScrollEventArgs>
        {
            private static readonly EventInfo EventInfo = typeof(MvxListView).GetEvent(nameof(MvxListView.Scroll));

            public ListViewScrollChangedEventSubscription(MvxListView source,
                                                           EventHandler<AbsListView.ScrollEventArgs> targetEventHandler)
            : base(source, EventInfo, targetEventHandler)
            {
            }

            protected override Delegate CreateEventHandler()
            {
                return new EventHandler<AbsListView.ScrollEventArgs>(base.OnSourceEvent);
            }
        }
        #endregion
    }
}
