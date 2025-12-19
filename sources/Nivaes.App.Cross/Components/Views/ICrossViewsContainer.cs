namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using MvvmCross.ViewModels;

    public interface ICrossViewsContainer : 
        ICrossViewFinder
    {
        void AddAll(IDictionary<Type, Type> viewModelViewLookup);

        void Add(Type viewModelType, Type viewType);

        void Add<TViewModel, TView>()
            where TViewModel : ICrossViewModel
            where TView : ICrossView;

        void AddSecondary(ICrossViewFinder finder);

        void SetLastResort(ICrossViewFinder finder);
    }
}
