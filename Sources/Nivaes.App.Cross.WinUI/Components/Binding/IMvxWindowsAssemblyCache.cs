namespace Nivaes.App.Cross.WinUI
{
    using System.Collections.Generic;
    using System.Reflection;

    public interface IMvxWindowsAssemblyCache
    {
        IList<Assembly> Assemblies { get; }
    }
}
