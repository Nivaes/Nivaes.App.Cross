namespace Nivaes.App.Cross
{
    using System;

    [Obsolete("No compatible con AoT")]
    public interface ICrossValueCombinerRegistry
        : ICrossNamedInstanceRegistry<ICrossValueCombiner>, ICrossValueCombinerLookup
    {
    }
}
