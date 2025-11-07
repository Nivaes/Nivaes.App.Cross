namespace Nivaes.App.Cross
{
    [Obsolete("No compatible con AoT", true)]
    public class CrossValueCombinerRegistry
        : CrossNamedInstanceRegistry<ICrossValueCombiner>, ICrossValueCombinerRegistry
    {
    }
}
