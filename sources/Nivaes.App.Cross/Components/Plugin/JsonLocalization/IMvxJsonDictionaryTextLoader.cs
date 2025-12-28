using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross;

public interface IMvxJsonDictionaryTextLoader
{
    [RequiresUnreferencedCode("MvxJsonConverter requires unreferenced code")]
    void LoadJsonFromResource(string namespaceKey, string typeKey, string resourcePath);

    [RequiresUnreferencedCode("MvxJsonConverter requires unreferenced code")]
    void LoadJsonFromText(string namespaceKey, string typeKey, string rawJson);
}
