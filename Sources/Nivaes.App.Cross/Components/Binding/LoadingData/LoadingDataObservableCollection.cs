using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Nivaes.App.Cross
{
    public class LoadingDataObservableCollection<T>
        : Collection<T>, ILoadingDataObservableCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private readonly Func<ValueTask<int>> mTotalItems;

        private readonly Func<int, int, Task<IEnumerable<T>>> mLoadDatas;

        private readonly int mPageSize;

        private bool mInsertItemFirstPosition;

        public int CountMargin => Math.Max(0, base.Count - mPageSize);

        public LoadingDataObservableCollection(Func<int, int, Task<IEnumerable<T>>> loadDatas, int pageSize = 20, bool insertItemFirstPosition = false)
        {
            mPageSize = pageSize;
            mLoadDatas = loadDatas;
            mInsertItemFirstPosition = insertItemFirstPosition;
        }

        public LoadingDataObservableCollection(Func<int, int, Task<IEnumerable<T>>> loadDatas, Func<ValueTask<int>> totalItems,
            int pageSize = 20, bool insertItemFirstPosition = false)
            : this(loadDatas, pageSize, insertItemFirstPosition)
        {
            mTotalItems = totalItems;
        }

        public ValueTask<int> TotalItems
        {
            get
            {
                if (mTotalItems != null)
                {
                    return mTotalItems.Invoke();
                }
                else
                {
                    return new ValueTask<int>(0);
                }
            }
        }

        public Task<int> LoadDatas()
        {
            return LoadDatas(mPageSize);
        }

        public async Task<int> LoadDatas(int pageSize)
        {
            var datas = await (mLoadDatas?.Invoke(Count, pageSize)).ConfigureAwait(false);

            if (datas != null)
            {
                foreach (var data in datas)
                {
                    if (mInsertItemFirstPosition)
                    {
                        base.Insert(0, data);
                    }
                    else
                    {
                        base.Add(data);
                    }
                }
            }

            return datas.Count();
        }

        protected override void ClearItems()
        {
            base.ClearItems();
        }

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));

            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
        }

        protected override void SetItem(int index, T item)
        {
            base.SetItem(index, item);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
