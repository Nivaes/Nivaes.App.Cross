// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Concurrent;
using System.Collections.Generic;
using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

namespace MvvmCross.Binding.Parse.PropertyPath
{
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
