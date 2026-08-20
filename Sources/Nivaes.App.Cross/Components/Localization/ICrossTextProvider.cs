namespace Nivaes.App.Cross
{
    public interface ICrossTextProvider
    {
        string? GetText(string namespaceKey, string typeKey, string name);

        string? GetText(string namespaceKey, string typeKey, string name, params object[] formatArgs);

        bool TryGetText(out string textValue, string namespaceKey, string typeKey, string name);

        bool TryGetText(out string textValue, string namespaceKey, string typeKey, string name, params object[] formatArgs);
    }
}
