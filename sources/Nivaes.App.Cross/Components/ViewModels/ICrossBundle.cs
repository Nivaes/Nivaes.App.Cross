namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;

    [Obsolete]
    public interface ICrossBundle
    {
        IDictionary<string, string> Data { get; }

        //void Write(object toStore);

        //T Read<T>() where T : new();

        //object Read(Type type);
    }
}
