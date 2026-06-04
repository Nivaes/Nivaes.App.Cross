namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public abstract class CrossViewsContainer
        : ICrossViewsContainer
    {
        //private readonly Dictionary<Type, Type> _bindingMap = [];
        //private readonly List<ICrossViewFinder> _secondaryViewFinders;
        //private ICrossViewFinder? _lastResortViewFinder;

        //protected CrossViewsContainer()
        //{
        //    _secondaryViewFinders = new List<ICrossViewFinder>();
        //}

        //[UnconditionalSuppressMessage("Trimming", "IL2072:UnrecognizedReflectionPattern",
        //    Justification = "Type annotations already guarantee that types have public constructors")]
        //public void AddAll(IDictionary<Type, Type> viewModelViewLookup)
        //{
        //    foreach (var pair in viewModelViewLookup)
        //    {
        //        Add(pair.Key, pair.Value);
        //    }
        //}

        //public void Add(Type viewModelType, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)] Type viewType)
        //{
        //    _bindingMap[viewModelType] = viewType;
        //}

        //public void Add<TViewModel, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)] TView>()
        //    where TViewModel : ICrossViewModel
        //    where TView : ICrossView
        //{
        //    Add(typeof(TViewModel), typeof(TView));
        //}

        // ToDo: Quitar esta clase y mover la busqueda de vistas.
        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)]
        public Type GetViewType(Type viewModelType)
        {
            var viewsManager = Singleton<CrossViewModelViewsManager>.Instance;
            if (viewsManager.TryGetValue(viewModelType, out var viewType))
            {
                return viewType;
            }

            throw new KeyNotFoundException("Could not find view for " + viewModelType);
        }
    }
}
