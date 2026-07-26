using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Nivaes.App.Cross.WinUI;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Nivaes.App.Cross.WinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow 
        : Window
    {
        public NavigationView NavigationView
        {
            get { return NavigationViewControl; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void RootGrid_Loaded(object sender, RoutedEventArgs e)
        {
            // We need to set the minimum size here because the XamlRoot is not available in the constructor.
            WindowHelper.SetWindowMinSize(this, 640, 500);

            if (sender is FrameworkElement rootGrid && rootGrid.XamlRoot is not null)
            {
                rootGrid.XamlRoot.Changed += RootGridXamlRoot_Changed;
            }

            //NavigationOrientationHelper.UpdateNavigationViewForElement(NavigationOrientationHelper.IsLeftMode());
            //TitleBarHelper.ApplySystemThemeToCaptionButtons(this, RootGrid.ActualTheme);
        }

        private void RootGrid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            //PointerPointProperties props = e.GetCurrentPoint(null).Properties;

            //if (props.IsXButton1Pressed)
            //{
            //    if (rootFrame.CanGoBack)
            //    {
            //        rootFrame.GoBack();
            //        e.Handled = true;
            //    }
            //}
            //else if (props.IsXButton2Pressed)
            //{
            //    if (rootFrame.CanGoForward)
            //    {
            //        rootFrame.GoForward();
            //        e.Handled = true;
            //    }
            //}
        }

        private void RootGridXamlRoot_Changed(XamlRoot sender, XamlRootChangedEventArgs args)
        {
            WindowHelper.SetWindowMinSize(this, 640, 500);
        }

        private void OnPaneDisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
        {
            if (sender.PaneDisplayMode == NavigationViewPaneDisplayMode.Top)
            {
                titleBar.IsPaneToggleButtonVisible = false;
            }
            else
            {
                titleBar.IsPaneToggleButtonVisible = true;
            }
        }

        private void OnNavigationViewControlLoaded(object sender, RoutedEventArgs e)
        {
            //// Delay necessary to ensure NavigationView visual state can match navigation
            //Task.Delay(500).ContinueWith(_ => this.NavigationViewLoaded?.Invoke(), TaskScheduler.FromCurrentSynchronizationContext());

            //var navigationView = sender as NavigationView;
            //navigationView?.RegisterPropertyChangedCallback(NavigationView.IsPaneOpenProperty, OnIsPaneOpenChanged);
        }
    }
}
