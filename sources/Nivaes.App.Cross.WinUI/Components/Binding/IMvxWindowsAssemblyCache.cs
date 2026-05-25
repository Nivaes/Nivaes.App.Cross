namespace Nivaes.App.Cross.WinUI3
{
    using System.Collections.Generic;
    using System.Reflection;

    public interface IMvxWindowsAssemblyCache
    {
        IList<Assembly> Assemblies { get; }
    }
}
