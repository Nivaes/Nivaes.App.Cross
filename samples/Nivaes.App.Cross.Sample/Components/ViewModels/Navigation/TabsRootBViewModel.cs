namespace Playground.Core.ViewModels
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    public class TabsRootBViewModel 
        : CrossNavigationViewModel
    {
        public TabsRootBViewModel(ILoggerFactory logProvider, ICrossNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            ShowInitialViewModelsCommand = new CrossAsyncCommand(ShowInitialViewModels);
        }

        public ICrossAsyncCommand ShowInitialViewModelsCommand { get; }

        private Task ShowInitialViewModels()
        {
            var tasks = new List<Task>();
            tasks.Add(NavigationService.Navigate<Tab1ViewModel, string>("test"));
            tasks.Add(NavigationService.Navigate<Tab2ViewModel>());
            return Task.WhenAll(tasks);
        }

        private int _itemIndex;

        public int ItemIndex
        {
            get { return _itemIndex; }
            set
            {
                if (_itemIndex == value) return;
                _itemIndex = value;
                Log.LogTrace("Tab item changed to {ItemIndex}", _itemIndex);
                RaisePropertyChanged(() => ItemIndex);
            }
        }
    }
}
