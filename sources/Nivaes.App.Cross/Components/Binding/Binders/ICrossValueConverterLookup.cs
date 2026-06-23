namespace Nivaes.App.Cross
{
    [Obsolete("", true)]
    public interface IMvxNamedInstanceLookup<out T>
    {
        T? Find(string name);
    }

    [Obsolete("", true)]
    public interface ICrossValueConverterLookup
        : IMvxNamedInstanceLookup<ICrossValueConverter>
    {
    }
}
