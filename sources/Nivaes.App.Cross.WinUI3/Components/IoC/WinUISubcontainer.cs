namespace Nivaes.App.Cross.WinUI3
{
    using Nivaes.IoC;

    public partial class WinUISubcontainer : IoCServiceContainer
    {
        protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
        {
            bootstrapper.AddSingleton<ICrossViewDispatcher, CrossWinUIViewDispatcher>();

            bootstrapper.AddSingleton<CrossPageViewPresentation>();
            bootstrapper.AddSingleton<CrossNewWindowViewPresentation>();
        }
    }
}
