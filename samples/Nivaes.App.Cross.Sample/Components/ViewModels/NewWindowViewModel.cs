namespace Nivaes.App.Cross.Sample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class NewWindowViewModel : CrossViewModel
    {
        private readonly ICrossNavigationService mNavigationService;

        public NewWindowViewModel(ICrossNavigationService navigationService)
        {
            mNavigationService = navigationService;
        }
    }
}
