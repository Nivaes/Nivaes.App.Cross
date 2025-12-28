namespace Nivaes.App.Cross.WinUI3
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;

    public class CrossWindowsContentDialog
        : ContentDialog
        , IMvxWindowsContentDialog
        , IDisposable
    {
        public CrossWindowsContentDialog()
        {
            Loading += MvxWindowsContentDialog_Loading;
            Loaded += MvxWindowsContentDialog_Loaded;
            Opened += MvxWindowsContentDialog_Opened;
            Closed += MvxWindowsContentDialog_Closed;
            Closing += MvxWindowsContentDialog_Closing;
            Unloaded += MvxWindowsContentDialog_Unloaded;
        }

        private void MvxWindowsContentDialog_Loading(FrameworkElement sender, object args)
        {
            ViewModel?.ViewAppearing();
        }

        private void MvxWindowsContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.ViewAppeared();
        }

        private void MvxWindowsContentDialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {
            ViewModel?.ViewCreated();
        }

        private void MvxWindowsContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            ViewModel?.ViewDisappearing();
        }

        private void MvxWindowsContentDialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            ViewModel?.ViewDisappeared();
        }

        private void MvxWindowsContentDialog_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.ViewDestroy();
        }

        public ICrossViewModel? ViewModel
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
                Loading -= MvxWindowsContentDialog_Loading;
                Loaded -= MvxWindowsContentDialog_Loaded;
                Opened -= MvxWindowsContentDialog_Opened;
                Closed -= MvxWindowsContentDialog_Closed;
                Closing -= MvxWindowsContentDialog_Closing;
                Unloaded -= MvxWindowsContentDialog_Unloaded;
            }
        }
    }

    public class CrossWindowsContentDialog<TViewModel>
        : CrossWindowsContentDialog
        , IMvxWindowsContentDialog<TViewModel> where TViewModel : class, ICrossViewModel
    {
        public new TViewModel ViewModel
        {
            get => (TViewModel)base.ViewModel;
            set => base.ViewModel = value;
        }
    }
}
