namespace Nivaes.App.Cross
{
    [Obsolete("No compatible con AoT", true)]
    public class CrossValueConverterRegistry
        : CrossNamedInstanceRegistry<ICrossValueConverter>, ICrossValueConverterLookup, ICrossValueConverterRegistry
    {
    }
}
