namespace Nivaes.App.Cross
{
    public interface IMvxNamedInstanceLookup<out T>
    {
        T? Find(string name);
    }

    public interface ICrossValueConverterLookup
        : IMvxNamedInstanceLookup<ICrossValueConverter>
    {
    }
}
