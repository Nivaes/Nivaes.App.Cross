namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class MixedNavTabsViewModel 
        : MvxNavigationViewModel
    {
        public MixedNavTabsViewModel(ILoggerFactory logProvider, ICrossNavigationService navigationService)
            : base(logProvider, navigationService)
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
