using System.ComponentModel;

namespace Nivaes.App.Cross;

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

    ValueTask Initialize();

    CrossNotifyTask? InitializeTask { get; set; }
}

public interface ICrossViewModel<in TParameter>
    : ICrossViewModel
{
    void Prepare(TParameter parameter);
}

public interface ICrossViewModelResult<out TResult>
    : ICrossViewModel
{
    TaskCompletionSource<object>? CloseCompletionSource { get; set; }
}

public interface ICrossViewModel<in TParameter, out TResult>
    : ICrossViewModel<TParameter>, ICrossViewModelResult<TResult>
{
}