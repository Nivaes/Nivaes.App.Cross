namespace MvvmCross.Binding.Combiners
{
    using Nivaes.App.Cross;

    public interface IMvxValueCombinerRegistry
        : ICrossNamedInstanceRegistry<IMvxValueCombiner>, IMvxValueCombinerLookup
    {
    }
}
