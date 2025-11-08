namespace Nivaes.App.Cross.WinUI3
{
    using System.Collections.Generic;
    using System.Reflection;

    public interface ICrossWindowsAssemblyCache
    {
        IList<Assembly> Assemblies { get; }
    }
}
