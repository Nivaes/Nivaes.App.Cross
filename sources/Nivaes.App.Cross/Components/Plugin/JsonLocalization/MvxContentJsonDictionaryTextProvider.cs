using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Nivaes.IoC;

namespace Nivaes.App.Cross;

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
