namespace Nivaes.App.Cross
{
    using System;

    public interface ICrossChildViewModelCache
    {
        int Cache(ICrossViewModel viewModel);

        ICrossViewModel? Get(int index);

        ICrossViewModel? Get(Type viewModelType);

        void Remove(int index);

        void Remove(Type viewModelType);

        bool Exists(Type viewModelType);
    }
}
