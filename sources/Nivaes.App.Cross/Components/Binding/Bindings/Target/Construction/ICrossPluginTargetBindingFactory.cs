namespace Nivaes.App.Cross
{
    using System.Collections.Generic;

    public interface ICrossPluginTargetBindingFactory
        : ICrossTargetBindingFactory
    {
        IEnumerable<CrossTypeAndNamePair> SupportedTypes { get; }
    }
}
