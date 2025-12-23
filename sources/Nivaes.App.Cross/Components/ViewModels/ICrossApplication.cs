namespace Nivaes.App.Cross
{
    using System.Threading.Tasks;

    public interface ICrossApplication 
        : ICrossViewModelLocatorCollection
    {
        void LoadPlugins(IMvxPluginManager pluginManager);

        void Initialize();

        Task Startup();

        void Reset();
    }

    public interface ICrossApplication<THint>
        : ICrossApplication
    {
        Task<THint> Startup(THint hint);
    }
}
