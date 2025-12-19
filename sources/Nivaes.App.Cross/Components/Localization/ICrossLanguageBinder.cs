namespace Nivaes.App.Cross
{
    [Obsolete]
    public interface ICrossLanguageBinder
    {
        string? GetText(string entryKey);

        string? GetText(string entryKey, params object[] args);
    }
}