namespace Nivaes.App.Cross
{
    using System.Collections.Concurrent;

    /// <summary>
    /// Stateless parser with global caching of tokens
    /// </summary>
    public class CrossSourcePropertyPathParser : ICrossSourcePropertyPathParser
    {
        private static readonly ConcurrentDictionary<string, IList<ICrossPropertyToken>> ParseCache =
            new ConcurrentDictionary<string, IList<ICrossPropertyToken>>();

        public IList<ICrossPropertyToken> Parse(string textToParse)
        {
            textToParse = CrossPropertyPathParser.MakeSafe(textToParse);
            if (ParseCache.TryGetValue(textToParse, out var cachedItem))
                return cachedItem;

            var parser = new CrossPropertyPathParser();
            var currentTokens = parser.Parse(textToParse);

            ParseCache.TryAdd(textToParse, currentTokens);
            return currentTokens;
        }
    }
}
