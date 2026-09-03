namespace Nivaes.App.Cross.UIKitLib
{
    public class LoadDataService : ILoadDataService
    {
        ICollection<T> ILoadDataService.GreateLoadingData<T>(Func<int, int, Task<IEnumerable<T>>> loadDatas, int page, bool insertItemFirstPosition)
        {
            return (new LoadingDataObservableCollection<T>(loadDatas, page, insertItemFirstPosition));
        }
    }
}
