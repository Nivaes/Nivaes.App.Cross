using Microsoft.Extensions.Logging;
using Nivaes.App.Cross;

namespace Playground.Core.ViewModels
{
    public class MixedNavTabsViewModel
        : CrossNavigationViewModel
    {
        public MixedNavTabsViewModel(ILogger<MixedNavTabsViewModel> logger)
            : base(logger)
        {
        }

        public override async void ViewAppearing()
        {
            await ShowInitialViewModels();
            base.ViewAppearing();
        }

        private Task ShowInitialViewModels()
        {
            var tasks = new List<Task>();
            tasks.Add(NavigationService.Navigate<MixedNavTab1ViewModel>());
            tasks.Add(NavigationService.Navigate<MixedNavTab2ViewModel>());
            //tasks.Add(NavigationService.Navigate<Tab1ViewModel, string>("test"));
            //tasks.Add(NavigationService.Navigate<Tab2ViewModel>());
            //tasks.Add(NavigationService.Navigate<Tab3ViewModel>());
            return Task.WhenAll(tasks);
        }
    }
}
