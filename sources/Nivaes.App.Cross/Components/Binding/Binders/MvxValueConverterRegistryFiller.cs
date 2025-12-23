namespace Nivaes.App.Cross
{
    using System;

    public class MvxValueConverterRegistryFiller
        : MvxNamedInstanceRegistryFiller<IMvxValueConverter>, IMvxValueConverterRegistryFiller
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
