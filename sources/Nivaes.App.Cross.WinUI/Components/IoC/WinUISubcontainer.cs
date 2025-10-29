namespace Nivaes.App.Cross.WinUI
{
    using Nivaes.App.Cross.WinUI.Presenters;
    using Nivaes.IoC;

    public partial class WinUISubcontainer : IoCServiceContainer
    {
        protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
        {
            bootstrapper.AddSingleton<ICrossViewDispatcher, WinUIViewDispatcher>();

            bootstrapper.AddSingleton<PageViewPresentation>();
            bootstrapper.AddSingleton<NewWindowViewPresentation>();
        }
    }
}
