namespace Nivaes.App.Cross.Sample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RootViewModel : ViewModel
    {
        private readonly INavigationService mNavigationService;

        public RootViewModel(INavigationService navigationService)
        {
            mNavigationService = navigationService;
        }
    }
}
