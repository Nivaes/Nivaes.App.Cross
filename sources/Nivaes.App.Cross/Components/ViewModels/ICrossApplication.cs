namespace Nivaes.App.Cross
{
    using System.Threading.Tasks;
    using MvvmCross.Plugin;
    using MvvmCross.ViewModels;

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
