
namespace Nivaes.App.Cross.WinUI
{
    public class LoadDataService
        : ILoadDataService
    {
        Task<ICollection<T>> ILoadDataService.GreateLoadingData<T>(Func<int, int, Task<IEnumerable<T>>> loadDatas, int page, bool insertItemFirstPosition)
        {
            return Task.FromResult<ICollection<T>>(new IncrementalLoadingCollection<T>(loadDatas, page, insertItemFirstPosition));
        }
    }
}
