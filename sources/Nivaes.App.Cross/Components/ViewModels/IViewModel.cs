namespace Nivaes.App.Cross
{
    using System.ComponentModel;

    public interface IViewModel
        : INotifyPropertyChanged
    {
        void ViewCreated();

        //void ViewAppearing();

        //void ViewAppeared();

        //void ViewDisappearing();

        //void ViewDisappeared();

        //void ViewDestroy(bool viewFinishing = true);

        //void Init(IBundle parameters);

        //void ReloadState(IBundle state);

        //void Start();

        //void SaveState(IBundle state);

        //void Prepare();

        //Task Initialize();

        //NotifyTask InitializeTask { get; set; }
    }

    public interface IViewModel<TParameter>
        : IViewModel
    {
        void Prepare(TParameter parameter);
    }

    public interface IViewModelResult<TResult> : IViewModel
    {
        TaskCompletionSource<object> CloseCompletionSource { get; set; }
    }

    public interface IViewModel<TParameter, TResult> 
        : IViewModel<TParameter>, IViewModelResult<TResult>
    {
    }
}
