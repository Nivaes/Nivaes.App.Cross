namespace Nivaes.App.Cross.Droid
{
    using System;  

    public interface IMvxMultipleViewModelCache
    {
        void Cache(ICrossViewModel toCache, string viewModelTag = "singleInstanceCache");

        ICrossViewModel GetAndClear(Type viewModelType, string viewModelTag = "singleInstanceCache");

        T GetAndClear<T>(string viewModelTag = "singleInstanceCache") where T : ICrossViewModel;
    }
}
