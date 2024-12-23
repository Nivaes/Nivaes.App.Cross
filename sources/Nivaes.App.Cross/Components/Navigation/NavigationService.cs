namespace Nivaes.App.Cross.Components.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class NavigationService : INavigationService
    {
        public Task Navigate<TViewModel>(CancellationToken cancellationToken = default)
            where TViewModel : IViewModel
        { 
            return Task.CompletedTask;
        }
    }
}
