using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nivaes.App.Cross.Sample
{
    public class SampleApplicationStart : CrossApplicationStart
    {
        INavigationService mNavigationService;

        public SampleApplicationStart(INavigationService navigationService)
        {
            mNavigationService = navigationService;
        }

        public override async Task NavigateToFirstViewModel(object? hint = null)
        {
            try
            {
                await mNavigationService.Navigate<ViewModel>();
            }
            catch (System.Exception exception)
            {
                //throw exception.Wrap("Problem navigating to ViewModel {0}", typeof(TViewModel).Name);
                throw exception.Wrap("Problem navigating to ViewModel {0}", typeof(ViewModel).Name);
            }
        }
    }
}
