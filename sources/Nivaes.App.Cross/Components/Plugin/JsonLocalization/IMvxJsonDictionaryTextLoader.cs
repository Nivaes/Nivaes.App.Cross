namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public interface IMvxJsonDictionaryTextLoader
    {
        [RequiresUnreferencedCode("MvxJsonConverter requires unreferenced code")]
        void LoadJsonFromResource(string namespaceKey, string typeKey, string resourcePath);

        [RequiresUnreferencedCode("MvxJsonConverter requires unreferenced code")]
        void LoadJsonFromText(string namespaceKey, string typeKey, string rawJson);
    }
}
