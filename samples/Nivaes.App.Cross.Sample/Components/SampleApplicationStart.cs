using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nivaes.App.Cross.Sample
{
    public class SampleApplicationStart : CrossApplicationStart
    {
        private readonly INavigationService mNavigationService;

        public SampleApplicationStart(INavigationService navigationService)
        {
            mNavigationService = navigationService;

            var container = Singleton<CrossIoCServiceContainer>.Instance;
            container.Merge(new ViewModelsSubcontainer());
        }

        public override async Task NavigateToFirstViewModel(object? hint = null)
        {
            try
            {
                await mNavigationService.Navigate<RootViewModel>();
            }
            catch (System.Exception exception)
            {
                //throw exception.Wrap("Problem navigating to ViewModel {0}", typeof(TViewModel).Name);
                throw exception.Wrap("Problem navigating to ViewModel {0}", typeof(ViewModel).Name);
            }
        }
    }
}
