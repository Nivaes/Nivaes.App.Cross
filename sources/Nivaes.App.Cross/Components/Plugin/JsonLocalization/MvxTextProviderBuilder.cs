using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross;

[RequiresUnreferencedCode("MvxJsonConverter requires unreferenced code")]
[Obsolete("¿Esto sirve para algo?")]
public abstract class MvxTextProviderBuilder
    : IMvxTextProviderBuilder
{
    protected readonly string _generalNamespaceKey;
    protected readonly string _rootFolderForResources;
    protected readonly IMvxJsonDictionaryTextLoader _textLoader;
    protected readonly ICrossTextProvider _textProvider;

    protected MvxTextProviderBuilder(string generalNamespaceKey, string rootFolderForResources)
        : this(generalNamespaceKey, rootFolderForResources, new MvxContentJsonDictionaryTextProvider())
    {
    }

    protected MvxTextProviderBuilder(string generalNamespaceKey, string rootFolderForResources, MvxJsonDictionaryTextProvider provider)
        : this(generalNamespaceKey, rootFolderForResources, provider, provider)
    {
    }

    protected MvxTextProviderBuilder(string generalNamespaceKey, string rootFolderForResources, IMvxJsonDictionaryTextLoader textLoader, ICrossTextProvider textProvider)
    {
        _generalNamespaceKey = generalNamespaceKey;
        _rootFolderForResources = rootFolderForResources;
        _textLoader = textLoader;
        _textProvider = textProvider;
        LoadResources(string.Empty);
    }

    protected abstract IDictionary<string, string> ResourceFiles { get; }

    public ICrossTextProvider TextProvider => _textProvider;

    public virtual void LoadResources(string whichLocalizationFolder)
    {
        foreach (var kvp in ResourceFiles)
        {
            try
            {
                var resourceFilePath = GetResourceFilePath(whichLocalizationFolder, kvp.Value);

                _textLoader.LoadJsonFromResource(
                    _generalNamespaceKey,
                    kvp.Key,
                    resourceFilePath);
            }
            catch (Exception exception)
            {
                //MvxPluginLog.Instance?.LogWarning(exception, "Language file could not be loaded for {FolderName}.{FileName}",
                //whichLocalizationFolder, kvp.Key);
            }
        }
    }

    protected virtual string GetResourceFilePath(string whichLocalizationFolder, string whichFile)
    {
        if (string.IsNullOrEmpty(whichLocalizationFolder))
            return $"{_rootFolderForResources}/{whichFile}.json";
        else
            return $"{_rootFolderForResources}/{whichLocalizationFolder}/{whichFile}.json";
    }
}
