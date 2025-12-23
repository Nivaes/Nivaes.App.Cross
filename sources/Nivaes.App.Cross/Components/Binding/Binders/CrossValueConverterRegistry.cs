namespace Nivaes.App.Cross
{
    public class CrossValueConverterRegistry
        : CrossNamedInstanceRegistry<ICrossValueConverter>, ICrossValueConverterLookup, ICrossValueConverterRegistry
    {
    }
}
