namespace Nivaes.App.Cross
{
    [Obsolete("", true)]
    public interface ICrossStringDictionaryParser
    {
        IDictionary<string, string> Parse(string textToParse);
    }
}
