namespace Nivaes.App.Cross
{
    [Obsolete("", true)]
    public class CrossValueConverterRegistry
        : CrossNamedInstanceRegistry<ICrossValueConverter>, ICrossValueConverterLookup, ICrossValueConverterRegistry
    {
    }
}
