namespace Nivaes.App.Cross
{
    [Obsolete("", true)]
    public interface ICrossValueCombinerRegistry
        : ICrossNamedInstanceRegistry<ICrossValueCombiner>, ICrossValueCombinerLookup
    {
    }
}
