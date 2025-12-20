namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.ViewModels;

    public abstract class CrossViewsContainer
        : ICrossViewsContainer
    {
        private readonly Dictionary<Type, Type> _bindingMap = [];
        private readonly List<ICrossViewFinder> _secondaryViewFinders;
        private ICrossViewFinder? _lastResortViewFinder;

        protected CrossViewsContainer()
        {
            _secondaryViewFinders = new List<ICrossViewFinder>();
        }

        [UnconditionalSuppressMessage("Trimming", "IL2072:UnrecognizedReflectionPattern",
            Justification = "Type annotations already guarantee that types have public constructors")]
        public void AddAll(IDictionary<Type, Type> viewModelViewLookup)
        {
            foreach (var pair in viewModelViewLookup)
            {
                Add(pair.Key, pair.Value);
            }
        }

        public void Add(Type viewModelType, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)] Type viewType)
        {
            _bindingMap[viewModelType] = viewType;
        }

        public void Add<TViewModel, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)] TView>()
            where TViewModel : IMvxViewModel
            where TView : ICrossView
        {
            Add(typeof(TViewModel), typeof(TView));
        }

        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)]
        public Type GetViewType(Type? viewModelType)
        {
            Type? binding;
            if (viewModelType != null && _bindingMap.TryGetValue(viewModelType, out binding))
            {
                return binding;
            }

            foreach (var viewFinder in _secondaryViewFinders)
            {
                binding = viewFinder.GetViewType(viewModelType);
                if (binding != null)
                {
                    return binding;
                }
            }

            if (_lastResortViewFinder != null)
            {
                binding = _lastResortViewFinder.GetViewType(viewModelType);
                if (binding != null)
                {
                    return binding;
                }
            }

            throw new KeyNotFoundException("Could not find view for " + viewModelType);
        }

        public void AddSecondary(ICrossViewFinder finder)
        {
            _secondaryViewFinders.Add(finder);
        }

        public void SetLastResort(ICrossViewFinder finder)
        {
            _lastResortViewFinder = finder;
        }
    }
}
