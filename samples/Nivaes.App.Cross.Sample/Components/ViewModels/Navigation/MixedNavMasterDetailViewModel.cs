using Microsoft.Extensions.Logging;
using Playground.Core.ViewModels;

namespace Nivaes.App.Cross.Sample;

public class MixedNavMasterDetailViewModel : CrossNavigationViewModel
{
    private MenuItem _menuItem;
    private ICrossAsyncCommand<MenuItem> _onSelectedChangedCommand;

    public class MenuItem
    {
        public string Title { get; set; }

        public string Description { get; set; }
        public Type ViewModelType { get; set; }
    }

    public MixedNavMasterDetailViewModel(ILoggerFactory logProvider, ICrossNavigationService navigationService)
        : base(logProvider, navigationService)
    {
        Menu = new[] {
            new MenuItem { Title = "Root", Description = "The root page", ViewModelType = typeof(MixedNavMasterRootContentViewModel) },
            new MenuItem { Title = "Tabs", Description = "Tabbed detail page", ViewModelType = typeof(MixedNavTabsViewModel)},
            new MenuItem { Title = "Result", Description = "Open detail page with result", ViewModelType = typeof(MixedNavResultDetailViewModel)},
        };
    }

    public IEnumerable<MenuItem> Menu { get; set; }

    //public MenuItem SelectedMenu
    //{
    //    get => _menuItem;
    //    set
    //    {
    //        if (SetProperty(ref _menuItem, value))
    //            OnSelectedChangedCommand.Execute(value);
    //    }
    //}

    //private ICrossAsyncCommand<MenuItem> OnSelectedChangedCommand
    //{
    //    get
    //    {
    //        return _onSelectedChangedCommand ??= new MvxAsyncCommand<MenuItem>(async (item) =>
    //        {
    //            if (item == null)
    //                return;

    //            var vmType = item.ViewModelType;
    //            await NavigationService.Navigate(vmType);
    //        });
    //    }
    //}
}
