namespace Nivaes.App.Cross.WinUI3
{
    using System;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;

    public class CrossWindowsContentDialog<TViewModel>
        : ContentDialog
        , ICrossWindowsContentDialog<TViewModel>
        , IDisposable
        where TViewModel : class, ICrossViewModel
    {
        public CrossWindowsContentDialog()
        {
            Loading += CrossWindowsContentDialog_Loading;
            Loaded += CrossWindowsContentDialog_Loaded;
            Opened += CrossWindowsContentDialog_Opened;
            Closed += CrossWindowsContentDialog_Closed;
            Closing += CrossWindowsContentDialog_Closing;
            Unloaded += CrossWindowsContentDialog_Unloaded;
        }

        #region ICrossView
        private TViewModel? _viewModel;

        public TViewModel? ViewModel
        {
            get => _viewModel;
            set
            {
                if (_viewModel == value)
                    return;

                _viewModel = value;
                DataContext = ViewModel;
                OnViewModelSet();
            }
        }

        ICrossViewModel? ICrossView.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (TViewModel?) value; 
        }
        #endregion


        private void CrossWindowsContentDialog_Loading(FrameworkElement sender, object args)
        {
            ViewModel?.ViewAppearing();
        }

        private void CrossWindowsContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.ViewAppeared();
        }

        private void CrossWindowsContentDialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {
            ViewModel?.ViewCreated();
        }

        private void CrossWindowsContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            ViewModel?.ViewDisappearing();
        }

        private void CrossWindowsContentDialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            ViewModel?.ViewDisappeared();
        }

        private void CrossWindowsContentDialog_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.ViewDestroy();
        }

        protected virtual void OnViewModelSet()
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~CrossWindowsContentDialog()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Loading -= CrossWindowsContentDialog_Loading;
                Loaded -= CrossWindowsContentDialog_Loaded;
                Opened -= CrossWindowsContentDialog_Opened;
                Closed -= CrossWindowsContentDialog_Closed;
                Closing -= CrossWindowsContentDialog_Closing;
                Unloaded -= CrossWindowsContentDialog_Unloaded;
            }
        }
    }
}
