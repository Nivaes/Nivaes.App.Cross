namespace Nivaes.App.Cross
{
    using System.Collections.Generic;

    public interface ICrossSourceBindingFactoryExtensionHost
    {
        IList<ICrossSourceBindingFactoryExtension> Extensions { get; }
    }
}
