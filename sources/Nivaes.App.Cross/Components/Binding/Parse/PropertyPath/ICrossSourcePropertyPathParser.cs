namespace Nivaes.App.Cross
{
    public interface ICrossSourcePropertyPathParser
    {
        IList<ICrossPropertyToken> Parse(string textToParse);
    }
}