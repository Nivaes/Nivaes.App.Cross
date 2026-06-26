namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    [Obsolete("", true)]
    public interface ICrossSetup
    {
        void InitializePrimary();

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        void InitializeSecondary();

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        IEnumerable<Assembly> GetViewAssemblies();
        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        IEnumerable<Assembly> GetViewModelAssemblies();
        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        IEnumerable<Assembly> GetPluginAssemblies();

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        IEnumerable<Type> CreatableTypes();

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        IEnumerable<Type> CreatableTypes(Assembly assembly);

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        void LoadPlugins(IMvxPluginManager pluginManager);

        event EventHandler<CrossSetupStateEventArgs>? StateChanged;
        CrossSetupState State { get; }
    }
#nullable restore
}
