using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Nivaes.App.Cross.WinUI
{
    public abstract class CrossWindowsContentDialog<TViewModel>
        : ContentDialog
        , ICrossWindowsContentDialog<TViewModel>
        , IDisposable
        where TViewModel : class, ICrossViewModel
    {
        public CrossWindowsContentDialog()
        {
            Loading += WindowsContentDialog_Loading;
            Loaded += WindowsContentDialog_Loaded;
            Opened += WindowsContentDialog_Opened;
            Closed += WindowsContentDialog_Closed;
            Closing += WindowsContentDialog_Closing;
            Unloaded += WindowsContentDialog_Unloaded;
        }

        private void WindowsContentDialog_Loading(FrameworkElement sender, object args)
        {
            ViewModel?.ViewAppearing();
        }

        private void WindowsContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.ViewAppeared();
        }

        private void WindowsContentDialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {
            ViewModel?.ViewCreated();
        }

        private void WindowsContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            ViewModel?.ViewDisappearing();
        }

        private void WindowsContentDialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            ViewModel?.ViewDisappeared();
        }

        private void WindowsContentDialog_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.ViewDestroy();
        }

        public TViewModel? ViewModel
        {
            get => field;
            set
            {
                if (field == value)
                    return;

                field = value;
                DataContext = ViewModel;
                OnViewModelSet();
            }
        }

        ICrossViewModel? ICrossView.ViewModel { get => ViewModel; set => ViewModel = (TViewModel?)value; }

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
                Loading -= WindowsContentDialog_Loading;
                Loaded -= WindowsContentDialog_Loaded;
                Opened -= WindowsContentDialog_Opened;
                Closed -= WindowsContentDialog_Closed;
                Closing -= WindowsContentDialog_Closing;
                Unloaded -= WindowsContentDialog_Unloaded;
            }
        }
    }
}
