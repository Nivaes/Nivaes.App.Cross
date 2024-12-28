using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nivaes.IoC;

namespace Nivaes.App.Cross.Sample
{
    public partial class ViewModelsIoCServiceContainer : IoCServiceContainer
    {
        protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
        {
            bootstrapper.AddSingleton<RootViewModel>();
        }
    }
}
