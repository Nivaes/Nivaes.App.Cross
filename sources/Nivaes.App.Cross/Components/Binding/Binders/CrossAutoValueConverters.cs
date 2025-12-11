namespace Nivaes.App.Cross
{
    public class CrossAutoValueConverters
            : ICrossAutoValueConverters
    {
        private record struct Key(Type ViewModelType, Type ViewType);

        private readonly Dictionary<Key, ICrossValueConverter> _lookup = new();

        public ICrossValueConverter Find(Type viewModelType, Type viewType)
        {
            _lookup.TryGetValue(new Key(viewModelType, viewType), out var result);
            return result ?? throw new KeyNotFoundException($"No converter registered for ViewModelType '{viewModelType}' and ViewType '{viewType}'.");
        }

        public void Register(Type viewModelType, Type viewType, ICrossValueConverter converter)
        {
            _lookup[new Key(viewModelType, viewType)] = converter;
        }
    }
}