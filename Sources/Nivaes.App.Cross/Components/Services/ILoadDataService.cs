namespace Nivaes.App.Cross;

public interface ILoadDataService
{
    Task<ICollection<T>> GreateLoadingData<T>(Func<int, int, Task<IEnumerable<T>>> loadDatas, int page = 20, bool insertItemFirstPosition = false);
}
