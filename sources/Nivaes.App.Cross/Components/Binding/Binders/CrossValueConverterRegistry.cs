namespace Nivaes.App.Cross
{
    using System;

    [Obsolete("No compatible con AoT", true)]
    public class CrossValueConverterRegistry
        : CrossNamedInstanceRegistry<ICrossValueConverter>, ICrossValueConverterLookup, ICrossValueConverterRegistry
    {
    }
}
