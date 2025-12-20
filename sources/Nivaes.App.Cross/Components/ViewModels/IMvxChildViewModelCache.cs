namespace MvvmCross.ViewModels
{
    using System;
    using Nivaes.App.Cross;

    public interface IMvxChildViewModelCache
    {
        int Cache(ICrossViewModel viewModel);

        ICrossViewModel? Get(int index);

        ICrossViewModel? Get(Type viewModelType);

        void Remove(int index);

        void Remove(Type viewModelType);

        bool Exists(Type viewModelType);
    }
}
