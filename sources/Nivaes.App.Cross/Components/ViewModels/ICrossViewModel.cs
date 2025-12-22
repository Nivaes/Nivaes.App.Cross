namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.ViewModels;

    public interface ICrossViewModel
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

    public interface ICrossViewModel<in TParameter>
        : ICrossViewModel
    {
        void Prepare(TParameter parameter);
    }
}
