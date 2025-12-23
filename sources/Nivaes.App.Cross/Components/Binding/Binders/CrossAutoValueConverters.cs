namespace Nivaes.App.Cross
{
    public class CrossAutoValueConverters
        : ICrossAutoValueConverters
    {
        private record struct Key(Type ViewModelType, Type ViewType);

        private readonly Dictionary<Key, IMvxValueConverter> _lookup = new();

        public IMvxValueConverter? Find(Type viewModelType, Type viewType)
        {
            _lookup.TryGetValue(new Key(viewModelType, viewType), out var result);
            return result;
        }

        public void Register(Type viewModelType, Type viewType, IMvxValueConverter converter)
        {
            _lookup[new Key(viewModelType, viewType)] = converter;
        }
    }
}