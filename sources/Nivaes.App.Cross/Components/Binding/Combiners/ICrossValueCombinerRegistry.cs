namespace Nivaes.App.Cross
{
    [Obsolete("No compatible con AoT")]
    public interface ICrossValueCombinerRegistry
        : ICrossNamedInstanceRegistry<ICrossValueCombiner>, ICrossValueCombinerLookup
    {
    }
}
