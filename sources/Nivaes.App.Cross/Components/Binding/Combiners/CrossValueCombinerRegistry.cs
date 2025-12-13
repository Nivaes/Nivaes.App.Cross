namespace Nivaes.App.Cross
{
    using System;

    [Obsolete("No compatible con AoT", true)]
    public class CrossValueCombinerRegistry
        : CrossNamedInstanceRegistry<ICrossValueCombiner>, ICrossValueCombinerRegistry
    {
    }
}
