namespace Nivaes.App.Cross
{

    public interface IMvxLanguageBinder
    {
        string? GetText(string entryKey);

        string? GetText(string entryKey, params object[] args);
    }
}