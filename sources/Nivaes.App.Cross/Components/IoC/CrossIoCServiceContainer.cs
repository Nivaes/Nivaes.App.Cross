namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Components.Navigation;
    using Nivaes.IoC;

    public partial class CrossIoCServiceContainer : IoCServiceContainer, ICrossIoCServiceContainer
    {
        protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
        {
            bootstrapper.AddSingleton<ICrossApplication, CrossApplication>();
            bootstrapper.AddSingleton<INavigationService, NavigationService>();
        }
    }
}
