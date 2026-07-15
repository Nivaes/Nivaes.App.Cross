using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross;

public class CrossSourceBindingFactory
    : ICrossSourceBindingFactory
    , ICrossSourceBindingFactoryExtensionHost
{
    private ICrossSourcePropertyPathParser? _propertyPathParser;


    protected ICrossSourcePropertyPathParser? SourcePropertyPathParser => _propertyPathParser ??= IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossSourcePropertyPathParser>();

    private readonly List<ICrossSourceBindingFactoryExtension> _extensions = [];

    //private readonly ICrossSourceBindingFactoryExtension _sourceBindingFactoryExtension;
    private readonly ILogger _logger;

    public CrossSourceBindingFactory(ILogger<CrossSourceBindingFactory> logger, ICrossSourceBindingFactoryExtension sourceBindingFactoryExtension)
    {
        //_sourceBindingFactoryExtension = sourceBindingFactoryExtension;
        _extensions.Add(sourceBindingFactoryExtension);
        _logger = logger;
    }

    [RequiresUnreferencedCode("This method uses reflection to create bindings, which may not be preserved in trimming scenarios")]
    protected bool TryCreateBindingFromExtensions(
        object source, ICrossPropertyToken propertyToken,
        List<ICrossPropertyToken> remainingTokens, out ICrossSourceBinding? result)
    {
        foreach (var extension in _extensions)
        {
            if (extension.TryCreateBinding(source, propertyToken, remainingTokens, out result))
            {
                return true;
            }
        }

        result = null;
        return false;
    }

    [RequiresUnreferencedCode("This method uses reflection to create bindings, which may not be preserved in trimming scenarios")]
    public ICrossSourceBinding? CreateBinding(object source, string combinedPropertyName)
    {
        var tokens = SourcePropertyPathParser?.Parse(combinedPropertyName);
        return CreateBinding(source, tokens);
    }

    [RequiresUnreferencedCode("This method uses reflection to create bindings, which may not be preserved in trimming scenarios")]
    public ICrossSourceBinding? CreateBinding(object source, IList<ICrossPropertyToken>? tokens)
    {
        if (tokens == null || tokens.Count == 0)
        {
            throw new CrossException("empty token list passed to CreateBinding");
        }

        var currentToken = tokens[0];
        var remainingTokens = tokens.Skip(1).ToList();
        if (TryCreateBindingFromExtensions(source, currentToken, remainingTokens, out ICrossSourceBinding? extensionResult))
        {
            return extensionResult;
        }

        if (source != null)
        {
            _logger.LogWarning(
                "Unable to bind: source property source not found @{CurrentToken} on {SourceTypeName}",
                currentToken,
                source.GetType().Name);
        }

        return new CrossMissingSourceBinding(source);
    }

    public IList<ICrossSourceBindingFactoryExtension> Extensions => _extensions;
}
