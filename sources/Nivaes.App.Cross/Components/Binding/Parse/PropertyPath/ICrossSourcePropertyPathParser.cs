namespace Nivaes.App.Cross
{
    using System.Collections.Generic;

    public interface ICrossSourcePropertyPathParser
    {
        IList<ICrossPropertyToken> Parse(string textToParse);
    }
}