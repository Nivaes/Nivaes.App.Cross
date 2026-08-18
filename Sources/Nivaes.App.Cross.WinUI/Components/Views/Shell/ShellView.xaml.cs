using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Nivaes.App.Cross;
using Nivaes.App.Cross.WinUI;
using Windows.System;
using Microsoft.UI.Input;

namespace Nivaes.App.Cross.WinUI
{
    [PagePresentation]
    public sealed partial class ShellView
        : ShellViewPage
    {
        private readonly Window _mainWindows;

        public Frame RootFrame => _rootFrame;

        //private NavigationView NavigationView => NavigationViewControl;
        //protected Frame PageContentCore => PageContent;
        private NavigationViewItem? _selectedItem;

        private List<NavigationViewItemBase>? _menuItems;

        public ShellView()
        {
            InitializeComponent();

            _mainWindows = ((CrossWinUIApplication)Microsoft.UI.Xaml.Application.Current).MainWindow!;

            _mainWindows!.ExtendsContentIntoTitleBar = true;
            _mainWindows!.SetTitleBar(_titleBar);
            //this.AppWindow.SetIcon("Assets/Tiles/GalleryIcon.ico");
            _mainWindows!.AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
        }

        #region RootGrid
        private void RootGrid_Loaded(object sender, RoutedEventArgs e)
        {
            // We need to set the minimum size here because the XamlRoot is not available in the constructor.
            _mainWindows.SetWindowMinSize(640, 500);

            if (sender is FrameworkElement rootGrid && rootGrid.XamlRoot is not null)
            {
                rootGrid.XamlRoot.Changed += RootGridXamlRoot_Changed;
            }

            //NavigationOrientationHelper.UpdateNavigationViewForElement(NavigationOrientationHelper.IsLeftMode());
            //TitleBarHelper.ApplySystemThemeToCaptionButtons(this, RootGrid.ActualTheme);
        }

        private void RootGrid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            PointerPointProperties props = e.GetCurrentPoint(null).Properties;

            if (props.IsXButton1Pressed)
            {
                if (_rootFrame.CanGoBack)
                {
                    _rootFrame.GoBack();
                    e.Handled = true;
                }
            }
            else if (props.IsXButton2Pressed)
            {
                if (_rootFrame.CanGoForward)
                {
                    _rootFrame.GoForward();
                    e.Handled = true;
                }
            }
        }

        private void RootGridXamlRoot_Changed(XamlRoot sender, XamlRootChangedEventArgs args)
        {
            _mainWindows.SetWindowMinSize(640, 500);
        }
        #endregion

        private void OnNavigationViewLoaded(object sender, RoutedEventArgs e)
        {
            if (!ViewModel!.IsLoaded)
            {
                CreateMenuItems();
                CreateMenuBack();

                ViewModel.IsLoaded = true;
            }
        }

        private void CreateMenuItems()
        {
            _menuItems = new List<NavigationViewItemBase>();

            using var itemEnumerator = ViewModel!.ShellModel.Items.GetEnumerator();
            var items = itemEnumerator.MoveNext();
            while (true)
            {
                foreach (var item in itemEnumerator.Current)
                {
                    var viewItem = new NavigationViewItem() 
                    {
                        Content = item.Label, 
                        Icon = item.Icon, 
                        Tag = item,
                        IsEnabled = item.Command != null
                    };

                    viewItem.Tapped += NavigationViewItemSelected;

                    _menuItems.Add(viewItem);
                }
                if (itemEnumerator.MoveNext())
                    _menuItems.Add(new NavigationViewItemSeparator());
                else
                    break;
            }

            _navigationView.MenuItemsSource = _menuItems;

            _selectedItem = _menuItems.FirstOrDefault() as NavigationViewItem;
            _navigationView.SelectedItem = _selectedItem;

            ((NavigationViewItem)_navigationView.SettingsItem).Tapped += NavigationViewItemSettingsItemSelected;
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
            _selectedItem = null;
            var showSettingsCommand = ViewModel!.ShellModel.ShowSettingsCommand;
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
                    if (menuItem.Reselectable || _selectedItem != item)
                    {
                        var command = menuItem.Command;
                        if (command != null)
                        {
                            await command.ExecuteAsync();
                        }
                        _selectedItem = item;
                        _navigationView.SelectedItem = item;
                    }
                }
        }

        private void BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (_navigationView.IsPaneOpen && (_navigationView.DisplayMode == NavigationViewDisplayMode.Compact || _navigationView.DisplayMode == NavigationViewDisplayMode.Minimal))
            {
                return;
            }
            else
            {
                if (_rootFrame.CanGoBack)
                {
                    _rootFrame.GoBack();
                }
            }

            args.Handled = true;
        }

        private async void OnContactNavigationViewItemTapped(object? sender, TappedRoutedEventArgs e)
        {
            await ViewModel!.ShellModel.ShowAccountCommand!.ExecuteAsync();
        }

        #region ControlSearch
        private void OnControlsSearchBoxTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            //if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            //{
            //    var suggestions = new List<ControlInfoDataItem>();

            //    var querySplit = sender.Text.Split(" ");
            //    foreach (var group in ControlInfoDataSource.Instance.Groups)
            //    {
            //        var matchingItems = group.Items.Where(
            //            item =>
            //            {
            //                // Idea: check for every word entered (separated by space) if it is in the name, 
            //                // e.g. for query "split button" the only result should "SplitButton" since its the only query to contain "split" and "button"
            //                // If any of the sub tokens is not in the string, we ignore the item. So the search gets more precise with more words
            //                bool flag = item.IncludedInBuild;
            //                foreach (string queryToken in querySplit)
            //                {
            //                    // Check if token is not in the title or any of the search tags
            //                    bool tokenMatches = item.Title.IndexOf(queryToken, StringComparison.CurrentCultureIgnoreCase) >= 0
            //                        || item.Tags.Any(tag => tag.IndexOf(queryToken, StringComparison.CurrentCultureIgnoreCase) >= 0);
            //                    if (!tokenMatches)
            //                    {
            //                        // Token is not in string, so we ignore this item.
            //                        flag = false;
            //                    }
            //                }
            //                return flag;
            //            });
            //        foreach (var item in matchingItems)
            //        {
            //            suggestions.Add(item);
            //        }
            //    }
            //    if (suggestions.Count > 0)
            //    {
            //        controlsSearchBox.ItemsSource = suggestions.OrderByDescending(i => i.Title.StartsWith(sender.Text, StringComparison.CurrentCultureIgnoreCase)).ThenBy(i => i.Title).ToList();
            //    }
            //    else
            //    {
            //        controlsSearchBox.ItemsSource = new string[] { "No results found" };
            //    }
            //}
        }

        private void OnControlsSearchBoxQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            //if (args.ChosenSuggestion is ControlInfoDataItem infoDataItem)
            //{
            //    var hasChangedSelection = EnsureItemIsVisibleInNavigation(infoDataItem.Title);

            //    // In case the menu selection has changed, it means that it has triggered
            //    // the selection changed event, that will navigate to the page already
            //    if (!hasChangedSelection)
            //    {
            //        Navigate(typeof(ItemPage), infoDataItem.UniqueId);
            //    }
            //}
            //else if (!string.IsNullOrEmpty(args.QueryText))
            //{
            //    Navigate(typeof(SearchResultsPage), args.QueryText);
            //}
        }

        private void CtrlF_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            controlsSearchBox.Focus(FocusState.Programmatic);
        }
        #endregion

        #region TitleBar
        private void TitleBar_PaneToggleRequested(TitleBar sender, object args)
        {
            _navigationView.IsPaneOpen = !_navigationView.IsPaneOpen;
        }

        private void TitleBar_BackRequested(TitleBar sender, object args)
        {
            if (this._rootFrame.CanGoBack)
            {
                this._rootFrame.GoBack();
            }
        }
        #endregion
    }

    public abstract class ShellViewPage
        : CrossWindowsPage<ShellViewModel>
    {
    }
}