namespace Nivaes.App.Cross
{
    public interface ICrossNamedInstanceLookup<out T>
    {
        T Find(string name);
    }

    public interface ICrossValueConverterLookup
        : ICrossNamedInstanceLookup<ICrossValueConverter>
    {
    }
}
