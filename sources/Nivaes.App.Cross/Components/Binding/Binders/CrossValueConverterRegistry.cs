namespace Nivaes.App.Cross
{
    public class CrossValueConverterRegistry
        : CrossNamedInstanceRegistry<IMvxValueConverter>, ICrossValueConverterLookup, IMvxValueConverterRegistry
    {
    }
}
