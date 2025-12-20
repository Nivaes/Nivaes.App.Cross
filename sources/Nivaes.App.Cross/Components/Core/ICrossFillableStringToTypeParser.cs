namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;

    [Obsolete]
    public interface ICrossFillableStringToTypeParser
    {
        IDictionary<Type, CrossStringToTypeParser.IParser> TypeParsers { get; }
        IList<CrossStringToTypeParser.IExtraParser> ExtraParsers { get; }
    }
}
