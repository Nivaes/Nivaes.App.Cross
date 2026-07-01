using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross;

[Obsolete("Esto se puede hacer de otra manera.")]
public interface IMvxJsonDictionaryTextLoader
{
    [RequiresUnreferencedCode("MvxJsonConverter requires unreferenced code")]
    void LoadJsonFromResource(string namespaceKey, string typeKey, string resourcePath);

    [RequiresUnreferencedCode("MvxJsonConverter requires unreferenced code")]
    void LoadJsonFromText(string namespaceKey, string typeKey, string rawJson);
}
