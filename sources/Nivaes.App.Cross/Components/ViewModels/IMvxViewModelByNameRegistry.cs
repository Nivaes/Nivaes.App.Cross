namespace MvvmCross.ViewModels
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Nivaes.App.Cross;

    public interface IMvxViewModelByNameRegistry
    {
        void Add(Type viewModelType);

        void Add<TViewModel>() 
            where TViewModel : ICrossViewModel;

        [RequiresUnreferencedCode("This method registers view models that may not be preserved by trimming")]
        void AddAll(Assembly assembly);
    }
}
