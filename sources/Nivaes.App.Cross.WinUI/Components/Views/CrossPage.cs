namespace Nivaes.App.Cross.WinUI
{
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Input;
    using Microsoft.UI.Xaml.Navigation;

    public abstract class CrossPage<TViewModel>
        : Page, IView, IDisposable
        where TViewModel : class, IViewModel
    {
        public CrossPage()
        {
            base.DataContextChanged += CrossPage_DataContextChanged;
        }

        private void CrossPage_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            
        }

        #region ViewModel
        private TViewModel? mViewModel;

        public TViewModel? ViewModel
        {
            get
            {
                return mViewModel;
            }
            set
            {
                if (mViewModel == value)
                    return;

                //base.Frame.DispatcherQueue.TryEnqueue(() =>
                //{
                //    mViewModel = value;
                //    DataContext = mViewModel;
                //    //RaisePropertyChanged();
                //    //RaisePropertyChanged(nameof(DataContext));
                //});

                mViewModel = value;
                DataContext = mViewModel;
                //RaisePropertyChanged();
                //RaisePropertyChanged(nameof(DataContext));
            }
        }
        #endregion

        protected override void OnBringIntoViewRequested(BringIntoViewRequestedEventArgs e)
        {
            base.OnBringIntoViewRequested(e);
        }

        protected override void OnCharacterReceived(CharacterReceivedRoutedEventArgs e)
        {
            base.OnCharacterReceived(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = (TViewModel)e.Parameter;

            base.OnNavigatedTo(e);

            ViewModel?.ViewCreated();
        }

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                base.DataContextChanged -= CrossPage_DataContextChanged;
                //Loading -= MvxWindowsPage_Loading;
                //Loaded -= MvxWindowsPage_Loaded;
                //Unloaded -= MvxWindowsPage_Unloaded;
            }
        }
        #endregion
    }
}
