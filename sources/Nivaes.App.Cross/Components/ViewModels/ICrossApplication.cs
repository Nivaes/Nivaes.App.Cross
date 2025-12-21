namespace Nivaes.App.Cross
{
    using System.Threading.Tasks;
    using MvvmCross.Plugin;
    using MvvmCross.ViewModels;

    public interface ICrossApplication 
        : IMvxViewModelLocatorCollection
    {
        void LoadPlugins(IMvxPluginManager pluginManager);

        void Initialize();

        Task Startup();

        void Reset();
    }

    public interface IMvxApplication<THint> : ICrossApplication
    {
        Task<THint> Startup(THint hint);
    }
}
