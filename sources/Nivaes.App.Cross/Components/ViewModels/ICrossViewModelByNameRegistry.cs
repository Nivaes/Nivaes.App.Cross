namespace Nivaes.App.Cross
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    [Obsolete("Quitar registro de typos")]
    public interface ICrossViewModelByNameRegistry
    {
        void Add(Type viewModelType);

        void Add<TViewModel>() 
            where TViewModel : ICrossViewModel;

        [RequiresUnreferencedCode("This method registers view models that may not be preserved by trimming")]
        void AddAll(Assembly assembly);
    }
}
