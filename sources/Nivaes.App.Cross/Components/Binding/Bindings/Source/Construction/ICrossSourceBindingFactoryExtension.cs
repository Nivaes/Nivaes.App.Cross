namespace Nivaes.App.Cross
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public interface ICrossSourceBindingFactoryExtension
    {
        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        bool TryCreateBinding(
            object source,
            ICrossPropertyToken propertyToken,
            List<ICrossPropertyToken> remainingTokens,
            out ICrossSourceBinding result);
    }
}
