namespace Nivaes.App.Cross
{
    [Obsolete("No compatible con AoT", true)]
    public class CrossValueConverterRegistryFiller
        : CrossNamedInstanceRegistryFiller<ICrossValueConverter>, ICrossValueConverterRegistryFiller
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
