using System.Collections.ObjectModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Windows.Foundation;
using Windows.UI.Core;

namespace Nivaes.App.Cross.WinUI
{

    public class IncrementalLoadingCollection<T>
         : ObservableCollection<T>, ISupportIncrementalLoading
    {
        private readonly Func<int, int, Task<IEnumerable<T>>> mLoadDatas;

        private readonly int mPage;

        private readonly bool mInsertItemFirstPosition;

        public IncrementalLoadingCollection(Func<int, int, Task<IEnumerable<T>>> loadDatas, int page = 20, bool insertItemFirstPosition = false)
        {
            mLoadDatas = loadDatas;
            mPage = page;
            mInsertItemFirstPosition = insertItemFirstPosition;
        }

        public bool HasMoreItems { get; private set; } = true;

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            var dispatcher = Window.Current.Dispatcher;

            return Task.Run(
                async () =>
                {
                    var datas = await mLoadDatas?.Invoke(base.Count, (int)count);
                    uint n = 0;
                    if (datas != null && datas.Any())
                    {
                        await dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                           () =>
                           {
                               foreach (var item in datas)
                               {
                                   if (mInsertItemFirstPosition)
                                   {
                                       base.Insert(0, item);
                                   }
                                   else
                                   {
                                       base.Add(item);
                                   }
                               }

                           });

                        n = (uint)datas.Count();
                    }
                    else
                    {
                        HasMoreItems = false;
                    }

                    return new LoadMoreItemsResult() { Count = n };
                }).AsAsyncOperation();
        }
    }
}
