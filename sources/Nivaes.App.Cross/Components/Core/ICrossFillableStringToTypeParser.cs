namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using MvvmCross.Core;

    public interface ICrossFillableStringToTypeParser
    {
        IDictionary<Type, CrossStringToTypeParser.IParser> TypeParsers { get; }
        IList<CrossStringToTypeParser.IExtraParser> ExtraParsers { get; }
    }
}
