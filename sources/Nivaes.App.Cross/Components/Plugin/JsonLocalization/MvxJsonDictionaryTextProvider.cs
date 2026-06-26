using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross;

public abstract class MvxJsonDictionaryTextProvider
    : MvxDictionaryTextProvider
     , IMvxJsonDictionaryTextLoader
{
    protected MvxJsonDictionaryTextProvider(bool maskErrors)
        : base(maskErrors)
    {
    }

    private ICrossJsonConverter _jsonConvert;
    protected ICrossJsonConverter JsonConvert
    {
        get
        {
            _jsonConvert = _jsonConvert ?? IPlatformApplication.Current!.Services.GetRequiredService<ICrossJsonConverter>();
            return _jsonConvert;
        }
    }

    [RequiresUnreferencedCode("MvxJsonConverter requires unreferenced code")]
    public abstract void LoadJsonFromResource(string namespaceKey, string typeKey, string resourcePath);

    [RequiresUnreferencedCode("MvxJsonConverter requires unreferenced code")]
    public virtual void LoadJsonFromText(string namespaceKey, string typeKey, string rawJson)
    {
        var entries = JsonConvert.DeserializeObject<Dictionary<string, string>>(rawJson);
        foreach (var kvp in entries)
        {
            AddOrReplace(namespaceKey, typeKey, kvp.Key, kvp.Value);
        }
    }
}
