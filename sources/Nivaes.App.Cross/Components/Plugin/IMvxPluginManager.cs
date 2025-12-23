namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public interface IMvxPluginManager
    {
        Func<Type, IMvxPluginConfiguration?> ConfigurationSource { get; }

        IEnumerable<Type> LoadedPlugins { get; }

        bool IsPluginLoaded(Type type);

        bool IsPluginLoaded<TPlugin>() where TPlugin : IMvxPlugin;

        void EnsurePluginLoaded([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type type, bool forceLoad = false);

        void EnsurePluginLoaded<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TPlugin>(bool forceLoad = false)
            where TPlugin : IMvxPlugin;

        bool TryEnsurePluginLoaded([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type type, bool forceLoad = false);

        bool TryEnsurePluginLoaded<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TPlugin>(bool forceLoad = false)
            where TPlugin : IMvxPlugin;
    }
}
