namespace Nivaes.App.Cross.AppKit
{
    using Nivaes.App.Cross.AppKit.Presenters;
    using Nivaes.IoC;

    public partial class AppKitSubcontainer : IoCServiceContainer
    {
        protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
        {
            bootstrapper.AddSingleton<ICrossViewDispatcher, AppKitViewDispatcher>();

            //bootstrapper.AddSingleton<PageViewPresentation>();
            //bootstrapper.AddSingleton<NewWindowViewPresentation>();
        }
    }
}
