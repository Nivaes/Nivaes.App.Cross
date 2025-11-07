namespace Nivaes.App.Cross
{
    [Obsolete("No compatible con AoT", true)]
    public class CrossValueCombinerRegistryFiller
        : CrossNamedInstanceRegistryFiller<ICrossValueCombiner>, ICrossValueCombinerRegistryFiller
    {
        public override string FindName(Type type)
        {
            var name = base.FindName(type);
            name = RemoveTail(name, "ValueCombiner");
            name = RemoveTail(name, "Combiner");
            return name;
        }
    }
}
