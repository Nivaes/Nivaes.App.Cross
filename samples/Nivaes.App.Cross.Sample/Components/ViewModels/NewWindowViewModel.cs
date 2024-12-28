namespace Nivaes.App.Cross.Sample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class NewWindowViewModel : ViewModel
    {
        private readonly INavigationService mNavigationService;

        public NewWindowViewModel(INavigationService navigationService)
        {
            mNavigationService = navigationService;
        }
    }
}
