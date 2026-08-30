
namespace Nivaes.App.Cross.WinUI
{
    public class LoadDataService
        : ILoadDataService
    {
        ICollection<T> ILoadDataService.GreateLoadingData<T>(Func<int, int, Task<IEnumerable<T>>> loadDatas, int page, bool insertItemFirstPosition)
        {
            return new IncrementalLoadingCollection<T>(loadDatas, page, insertItemFirstPosition);
        }
    }
}
