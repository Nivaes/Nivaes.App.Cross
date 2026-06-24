namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;

    public abstract class CrossViewsContainer
        : ICrossViewsContainer
    {
        protected ILogger Logger { get; }

        public CrossViewsContainer(ILogger logger)
        {
            Logger = logger;
        }

        // ToDo: Quitar esta clase y mover la busqueda de vistas.
        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)]
        public Type GetViewType(Type viewModelType)
        {
            var viewsManager = Singleton<CrossViewModelViewsManager>.Instance;
            if (viewsManager.TryGetValue(viewModelType, out var viewType))
            {
                return viewType;
            }

            throw new CrossException($"Could not find view for {viewModelType}");
        }
    }
}
