using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Nivaes.App.Cross;
using Nivaes.App.Cross.WinUI;
using Windows.System;

namespace Nivaes.App.Cross.WinUI
{
    [PagePresentation]
    public sealed partial class ShellView
        : ShellViewPage
    {
        //protected NavigationView NavigationViewCore => NavigationView;
        //protected Frame PageContentCore => PageContent;
        private NavigationViewItem mSelectedItem;

        private List<NavigationViewItemBase> mMenuItem;

        public ShellView()
        {
            InitializeComponent();

            var mainWindows = (Microsoft.UI.Xaml.Application.Current as CrossWinUIApplication)?.MainWindow;

            mainWindows.ExtendsContentIntoTitleBar = true;
            //mainWindows.SetTitleBar(titleBar);
            //this.AppWindow.SetIcon("Assets/Tiles/GalleryIcon.ico");
            mainWindows.AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
        }

        protected void OnNavigationViewLoaded(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsLoaded)
            {
                CreateMenuItems();
                CreateMenuBack();

                ViewModel.IsLoaded = true;
            }
        }

        private void CreateMenuItems()
        {
            mMenuItem = new List<NavigationViewItemBase>();

            foreach (var item in ViewModel.ShellModel.PrimaryItems)
            {
                var viewItem = new NavigationViewItem() { Content = item.Label, Icon = item.Icon, Tag = item };

                viewItem.Tapped += NavigationViewItemSelected;

                mMenuItem.Add(viewItem);
            }

            mMenuItem.Add(new NavigationViewItemSeparator());

            foreach (var item in ViewModel.ShellModel.SecondaryItems)
            {
                var viewItem = new NavigationViewItem() { Content = item.Label, Icon = item.Icon, Tag = item };

                viewItem.Tapped += NavigationViewItemSelected;

                mMenuItem.Add(viewItem);
            }

            NavigationView.MenuItemsSource = mMenuItem;

            mSelectedItem = (NavigationViewItem)mMenuItem.FirstOrDefault();
            NavigationView.SelectedItem = mSelectedItem;

            ((NavigationViewItem)NavigationView.SettingsItem).Tapped += NavigationViewItemSettingsItemSelected;
        }

        private void CreateMenuBack()
        {
            KeyboardAccelerator GoBack = new KeyboardAccelerator
            {
                Key = VirtualKey.GoBack
            };
            GoBack.Invoked += BackInvoked;

            KeyboardAccelerator AltLeft = new KeyboardAccelerator
            {
                Key = VirtualKey.Left
            };
            AltLeft.Invoked += BackInvoked;

            base.KeyboardAccelerators.Add(GoBack);
            base.KeyboardAccelerators.Add(AltLeft);

            AltLeft.Modifiers = VirtualKeyModifiers.Menu;
        }

        private async void NavigationViewItemSettingsItemSelected(object sender, TappedRoutedEventArgs e)
        {
            var showSettingsCommand = ViewModel.ShellModel.ShowSettingsCommand;
            if (showSettingsCommand != null && showSettingsCommand.CanExecute())
            {
                await showSettingsCommand.ExecuteAsync();
            }
        }

        private async void NavigationViewItemSelected(object sender, TappedRoutedEventArgs e)
        {
            var item = (NavigationViewItem)sender;

            if (item.Tag is ShellNavigationItem menuItem)
            {
                if (menuItem.Reselectable || mSelectedItem != item)
                {
                    var command = menuItem.Command;
                    await command.ExecuteAsync();

                    mSelectedItem = item;
                    NavigationView.SelectedItem = item;
                }
            }
        }

        private void BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (NavigationView.IsPaneOpen && (NavigationView.DisplayMode == NavigationViewDisplayMode.Compact || NavigationView.DisplayMode == NavigationViewDisplayMode.Minimal))
            {
                return;
            }
            else
            {
                if (PageContent.CanGoBack)
                {
                    PageContent.GoBack();
                }
            }

            args.Handled = true;
        }

        private async void OnContactNavigationViewItemTapped(object? sender, TappedRoutedEventArgs e)
        {
            await ViewModel!.ShellModel.ShowAccountCommand!.ExecuteAsync();
        }
    }

    public abstract class ShellViewPage
        : BaseWindowsPage<ShellViewModel>
    {
    }
}