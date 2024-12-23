namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface INavigationService
    {
        Task Navigate<TViewModel>(CancellationToken cancellationToken = default)
            where TViewModel : IViewModel;
    }
}
