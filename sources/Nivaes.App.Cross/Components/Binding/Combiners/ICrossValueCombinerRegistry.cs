namespace Nivaes.App.Cross
{
    public interface ICrossValueCombinerRegistry
        : ICrossNamedInstanceRegistry<ICrossValueCombiner>, ICrossValueCombinerLookup
    {
    }
}
