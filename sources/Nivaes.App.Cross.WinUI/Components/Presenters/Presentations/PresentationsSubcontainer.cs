namespace Nivaes.App.Cross.Sample.Presentations
{
    using Nivaes.App.Cross.WinUI.Presenters;
    using Nivaes.IoC;

    public partial class PresentationsSubcontainer : IoCServiceContainer
    {
        protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
        {
            bootstrapper.AddSingleton<WinUIPageViewPresentation>();
            bootstrapper.AddSingleton<WinUINewWindowViewPresentation>();
        }
    }
}
