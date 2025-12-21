namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using MvvmCross.Core;

    public interface ICrossFillableStringToTypeParser
    {
        IDictionary<Type, MvxStringToTypeParser.IParser> TypeParsers { get; }
        IList<MvxStringToTypeParser.IExtraParser> ExtraParsers { get; }
    }
}
