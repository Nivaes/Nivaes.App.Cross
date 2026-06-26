using Nivaes.IoC;

namespace Nivaes.App.Cross.WinUI;

public partial class WinUISubcontainer : IoCServiceContainer
{
    protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
    {
        //bootstrapper.AddSingleton<ICrossViewDispatcher, CrossWinUIViewDispatcher>();

        //bootstrapper.AddSingleton<CrossPageViewPresentation>();
        //bootstrapper.AddSingleton<CrossNewWindowViewPresentation>();
        bootstrapper.AddSingleton<ICrossSuspensionManager, CrossSuspensionManager>();
        bootstrapper.AddSingleton<ICrossWindowsViewModelRequestTranslator, CrossWindowsViewsContainer>();
    }
}
