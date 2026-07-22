namespace Nivaes.App.Cross.UIKitLib
{
    public class LoadDataService : ILoadDataService
    {
        Task<ICollection<T>> ILoadDataService.GreateLoadingData<T>(Func<int, int, Task<IEnumerable<T>>> loadDatas, int page, bool insertItemFirstPosition)
        {
            return Task.FromResult<ICollection<T>>(new LoadingDataObservableCollection<T>(loadDatas, page, insertItemFirstPosition));
        }
    }
}
