namespace Nivaes.App.Cross
{
    public interface ICrossAutoValueConverters
    {
        IMvxValueConverter? Find(Type viewModelType, Type viewType);

        void Register(Type viewModelType, Type viewType, IMvxValueConverter converter);
    }
}