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

        void Init(IMvxBundle parameters);

        void ReloadState(IMvxBundle state);

        void Start();

        void SaveState(IMvxBundle state);

        void Prepare();

        Task Initialize();

        MvxNotifyTask? InitializeTask { get; set; }
    }

    public interface IMvxViewModel<in TParameter>
        : ICrossViewModel
    {
        void Prepare(TParameter parameter);
    }
}
