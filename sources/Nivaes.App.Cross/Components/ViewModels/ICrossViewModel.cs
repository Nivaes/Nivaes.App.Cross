namespace Nivaes.App.Cross
{
    using System.ComponentModel;
    using System.Threading.Tasks;

    public interface ICrossViewModel
        : INotifyPropertyChanged
    {
        void ViewCreated();

        void ViewAppearing();

        void ViewAppeared();

        void ViewDisappearing();

        void ViewDisappeared();

        void ViewDestroy(bool viewFinishing = true);

        void Init(ICrossBundle parameters);

        void ReloadState(ICrossBundle state);

        void Start();

        void SaveState(ICrossBundle state);

        void Prepare();

        Task Initialize();

        CrossNotifyTask? InitializeTask { get; set; }
    }

    public interface ICrossViewModel<TParameter>
        : ICrossViewModel
    {
        void Prepare(TParameter parameter);
    }

    public interface ICrossViewModelResult<TResult> 
        : ICrossViewModel
    {
        TaskCompletionSource<object>? CloseCompletionSource { get; set; }
    }

    public interface ICrossViewModel<TParameter, TResult> 
        : ICrossViewModel<TParameter>, ICrossViewModelResult<TResult>
    {
    }
}
