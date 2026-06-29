using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross;

[Obsolete("Json", true)]
public class MvxContentJsonDictionaryTextProvider
    : MvxJsonDictionaryTextProvider
{
    private ICrossResourceLoader _resourceLoader;

    protected ICrossResourceLoader ResourceLoader
    {
        get
        {
            _resourceLoader = _resourceLoader ?? IPlatformApplication.Current!.Services.GetRequiredService<ICrossResourceLoader>();
            return _resourceLoader;
        }
    }

    public MvxContentJsonDictionaryTextProvider(bool maskErrors = true)
        : base(maskErrors)
    {
    }

    [RequiresUnreferencedCode("MvxJsonConverter requires unreferenced code")]
    public override void LoadJsonFromResource(string namespaceKey, string typeKey, string resourcePath)
    {
        var service = ResourceLoader;
        var json = service.GetTextResource(resourcePath);
        if (string.IsNullOrEmpty(json))
            throw new FileNotFoundException("Unable to find resource file " + resourcePath);
        LoadJsonFromText(namespaceKey, typeKey, json);
    }
}
