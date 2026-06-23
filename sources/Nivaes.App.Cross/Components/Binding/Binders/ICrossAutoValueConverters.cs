namespace Nivaes.App.Cross
{
    [Obsolete("", true)]
    public interface ICrossAutoValueConverters
    {
        ICrossValueConverter? Find(Type viewModelType, Type viewType);

        void Register(Type viewModelType, Type viewType, ICrossValueConverter converter);
    }
}