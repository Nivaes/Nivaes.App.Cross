namespace Nivaes.App.Cross
{
    [Obsolete("No compatible con AoT", true)]
    public interface ICrossValueCombinerRegistry
        : ICrossNamedInstanceRegistry<ICrossValueCombiner>, ICrossValueCombinerLookup
    {
    }
}
