using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross;

namespace Playground.Core.Services;

[RequiresUnreferencedCode("MvxTextProvider requires unreferenced code")]
public class TextProviderBuilder : MvxTextProviderBuilder
{
    public TextProviderBuilder() : base("Playground.Core", "Resources", new MvxEmbeddedJsonDictionaryTextProvider(false))
    {
    }

    protected override IDictionary<string, string> ResourceFiles
    {
        get
        {
            return new Dictionary<string, string>
                {
                    { "Text", "Text" }
                };
        }
    }
}
