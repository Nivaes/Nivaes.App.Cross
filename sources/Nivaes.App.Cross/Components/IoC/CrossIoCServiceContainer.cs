using Nivaes.IoC;

namespace Nivaes.App.Cross;

[Obsolete("This class is deprecated", true)]
public partial class CrossIoCServiceContainer : IoCServiceContainer, ICrossIoCServiceContainer
{
    protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
    {
        ////bootstrapper.AddSingleton<ILoggingService, LoggingService>();
        //bootstrapper.AddSingleton<ICrossNavigationService, CrossNavigationService>();

        //bootstrapper.AddSingleton<ICrossSettings, CrossSettings>();
        //bootstrapper.AddSingleton<ICrossStringToTypeParser, CrossStringToTypeParser>();
        //bootstrapper.AddSingleton<ICrossViewModelLoader, CrossViewModelLoader>();
        //bootstrapper.AddSingleton<ICrossResultViewModelManager, CrossResultViewModelManager>();
        ////bootstrapper.AddSingleton<ICrossViewModelTypeFinder, CrossViewModelViewTypeFinder>();
        ////bootstrapper.AddSingleton<ICrossViewModelByNameLookup, CrossViewModelByNameLookup>();
        ////bootstrapper.AddSingleton<ICrossViewModelByNameRegistry, CrossViewModelByNameLookup>();
        ////bootstrapper.AddSingleton<ICrossTypeToTypeLookupBuilder, CrossViewModelViewLookupBuilder>();
        //bootstrapper.AddSingleton<ICrossCommandCollectionBuilder, CrossCommandCollectionBuilder>();
        //bootstrapper.AddSingleton<ICrossNavigationSerializer, CrossStringDictionaryNavigationSerializer>();
        //bootstrapper.AddSingleton<ICrossChildViewModelCache, CrossChildViewModelCache>();
    }
}
