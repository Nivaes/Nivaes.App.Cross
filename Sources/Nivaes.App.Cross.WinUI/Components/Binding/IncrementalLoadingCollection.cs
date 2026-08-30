using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Windows.Foundation;

namespace Nivaes.App.Cross.WinUI
{

    public class IncrementalLoadingCollection<T>
         : ObservableCollection<T>, ISupportIncrementalLoading
    {
        private readonly Func<int, int, Task<IEnumerable<T>>> _loadDatas;

        private readonly int _page;

        private readonly bool _insertItemFirstPosition;

        public IncrementalLoadingCollection(Func<int, int, Task<IEnumerable<T>>> loadDatas, int page = 20, bool insertItemFirstPosition = false)
        {
            _loadDatas = loadDatas;
            _page = page;
            _insertItemFirstPosition = insertItemFirstPosition;
        }

        public bool HasMoreItems { get; private set; } = true;

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            var dispatcher = (Application.Current as CrossWinUIApplication)?.MainWindow?.DispatcherQueue;

            if (dispatcher == null)
                throw new AppException($"App must inherit from {nameof(CrossWinUIApplication)}");

            var loadCount = count == 1 ? 20 : count;

            return AsyncInfo.Run<LoadMoreItemsResult>(async cancellationToken =>
                {
                    var datas = await _loadDatas.Invoke(base.Count, (int)loadCount);
                    uint n = 0;
                    if (datas != null && datas.Any())
                    {
                        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

                        dispatcher.TryEnqueue(DispatcherQueuePriority.Normal, () =>
                           {
                               try
                               {
                                   foreach (var item in datas)
                                   {
                                       if (_insertItemFirstPosition)
                                       {
                                           base.Insert(0, item);
                                       }
                                       else
                                       {
                                           base.Add(item);
                                       }
                                   }
                                   tcs.SetResult();
                               }
                               catch (Exception ex)
                               {
                                   tcs.SetException(new AppException("Failed to load more items.", ex));
                               }

                           });

                        await tcs.Task;

                        n = (uint)datas.Count();
                    }
                    else
                    {
                        HasMoreItems = false;
                    }

                    return new LoadMoreItemsResult() { Count = n };
                });
        }
    }
}
