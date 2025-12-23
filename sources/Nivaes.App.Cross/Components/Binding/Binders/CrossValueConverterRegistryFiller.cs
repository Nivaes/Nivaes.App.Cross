namespace Nivaes.App.Cross
{
    using System;

    public class CrossValueConverterRegistryFiller
        : CrossNamedInstanceRegistryFiller<IMvxValueConverter>, ICrossValueConverterRegistryFiller
    {
        public override string FindName(Type type)
        {
            var name = base.FindName(type);
            name = RemoveTail(name, "ValueConverter");
            name = RemoveTail(name, "Converter");
            return name;
        }
    }
}
