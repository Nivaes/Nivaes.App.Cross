namespace Nivaes.App.Cross
{
    public interface ICrossStringDictionaryParser
    {
        IDictionary<string, string> Parse(string textToParse);
    }
}
