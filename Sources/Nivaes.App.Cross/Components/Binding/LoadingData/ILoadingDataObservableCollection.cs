using System.Collections.Specialized;
using System.ComponentModel;

namespace Nivaes.App.Cross
{
    public interface ILoadingDataObservableCollection
        : INotifyCollectionChanged, INotifyPropertyChanged
    {
        ValueTask<int> TotalItems { get; }

        int CountMargin { get; }

        Task<int> LoadDatas();

        Task<int> LoadDatas(int pageSize);
    }

    public interface ILoadingDataObservableCollection<T>
        : ICollection<T>, ILoadingDataObservableCollection, INotifyCollectionChanged, INotifyPropertyChanged
    {
    }
}
