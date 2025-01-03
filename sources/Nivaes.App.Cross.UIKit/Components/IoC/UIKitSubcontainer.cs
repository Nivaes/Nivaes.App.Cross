namespace Nivaes.App.Cross.UIKit
{
    using Nivaes.App.Cross.UIKit.Presenters;
    using Nivaes.IoC;

    public partial class UIKitSubcontainer : IoCServiceContainer
    {
        protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
        {
            bootstrapper.AddSingleton<IViewDispatcher, UIKitViewDispatcher>();

            //bootstrapper.AddSingleton<PageViewPresentation>();
            //bootstrapper.AddSingleton<NewWindowViewPresentation>();
        }
    }
}
