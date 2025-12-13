namespace Nivaes.App.Cross
{
    using System;

    [Obsolete("No compatible con AoT", true)]
    public interface ICrossValueCombinerRegistryFiller
        : ICrossNamedInstanceRegistryFiller<ICrossValueCombiner>
    {
    }
}
