namespace MvvmCross.Platforms.Android.Views
{
    using System;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public interface IMvxMultipleViewModelCache
    {
        void Cache(ICrossViewModel toCache, string viewModelTag = "singleInstanceCache");

        ICrossViewModel GetAndClear(Type viewModelType, string viewModelTag = "singleInstanceCache");

        T GetAndClear<T>(string viewModelTag = "singleInstanceCache") where T : ICrossViewModel;
    }
}
