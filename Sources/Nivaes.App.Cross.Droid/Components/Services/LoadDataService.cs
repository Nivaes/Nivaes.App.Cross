namespace Nivaes.App.Cross.Droid
{
    public class LoadDataService
        : ILoadDataService
    {
        ICollection<T> ILoadDataService.GreateLoadingData<T>(Func<int, int, Task<IEnumerable<T>>> loadDatas, int page, bool insertItemFirstPosition)
        {
            return new LoadingDataObservableCollection<T>(loadDatas, page, insertItemFirstPosition);
        }
    }
}
