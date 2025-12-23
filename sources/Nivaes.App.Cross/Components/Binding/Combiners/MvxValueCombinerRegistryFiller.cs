namespace Nivaes.App.Cross
{
    public class MvxValueCombinerRegistryFiller
        : MvxNamedInstanceRegistryFiller<IMvxValueCombiner>, IMvxValueCombinerRegistryFiller
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
