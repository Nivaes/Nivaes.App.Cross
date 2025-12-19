namespace Nivaes.App.Cross.WinUI3
{
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.Intrinsics.X86;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Input;
    using Microsoft.UI.Xaml.Navigation;
    using Windows.UI.Core;
    using Nivaes.IoC;

    public abstract class CrossWindowsPage<TViewModel>
        : Page, ICrossView<TViewModel>, IDisposable
        where TViewModel : class, ICrossViewModel
    {
        public CrossWindowsPage()
        {
            Loading += CrossWindowsPage_Loading;
            Loaded += CrossWindowsPage_Loaded;
            Unloaded += CrossWindowsPage_Unloaded;
        }

        private void CrossWindowsPage_Loading(FrameworkElement sender, object args)
        {
            ViewModel?.ViewAppearing();
        }

        private void CrossWindowsPage_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.ViewAppeared();
        }

        private void CrossWindowsPage_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.ViewDestroy();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewModel?.ViewDisappearing();
            base.OnNavigatingFrom(e);
        }

        private ICrossViewModel? _viewModel;

        public ICrossWindowsFrame WrappedFrame => new CrossWrappedFrame(Frame);

        #region ICrossView
        public TViewModel? ViewModel
        {
            get
            {
                return (TViewModel)DataContext;
            }
            set
            {
                DataContext = value;
            }
        }

        ICrossViewModel? ICrossView.ViewModel 
        { 
            get => ViewModel; 
            set => ViewModel = (TViewModel?)value; 
        }
        #endregion

        protected virtual void OnViewModelSet()
        {
        }

        public virtual void ClearBackStack()
        {
            var backStack = base.Frame?.BackStack;

            while (backStack != null && backStack.Any())
            {
                backStack.RemoveAt(0);
            }

            UpdateBackButtonVisibility();
        }

        protected virtual void UpdateBackButtonVisibility()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private string _reqData = string.Empty;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ViewModel?.ViewCreated();

            if (_reqData != string.Empty)
            {
                var container = Singleton<CrossViewPresentationsManager>.Instance;

                //var viewModelLoader = Cross.IoCProvider.Resolve<ICrossWindowsViewModelLoader>();
                //ViewModel = viewModelLoader.Load(e.Parameter.ToString(), LoadStateBundle(e));
                //ViewModel?.ViewCreated();
            }
            _reqData = (string)e.Parameter;

            //this.OnViewCreate(_reqData, () => LoadStateBundle(e));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel?.ViewDisappeared();

            throw new NotImplementedException();

            //var bundle = this.CreateSaveStateBundle();
            //SaveStateBundle(e, bundle);

            //var translator = Cross.IoCProvider.Resolve<ICrossWindowsViewModelRequestTranslator>();

            //if (e.NavigationMode == NavigationMode.Back)
            //{
            //    var key = translator.RequestTextGetKey(_reqData);
            //    this.OnViewDestroy(key);
            //}
            //else
            //{
            //    var backstack = Frame.BackStack;
            //    if (backstack.Count > 0)
            //    {
            //        var currentEntry = backstack[backstack.Count - 1];
            //        var key = translator.RequestTextGetKey(currentEntry.Parameter.ToString());
            //        if (key == 0)
            //        {
            //            var newParamter = translator.GetRequestTextWithKeyFor(ViewModel);
            //            var entry = new PageStackEntry(currentEntry.SourcePageType, newParamter, currentEntry.NavigationTransitionInfo);
            //            backstack.Remove(currentEntry);
            //            backstack.Add(entry);
            //        }
            //    }
            //}
        }

        private string _pageKey;

        private ICrossSuspensionManager _suspensionManager;
        protected ICrossSuspensionManager SuspensionManager
        {
            get
            {
                throw new NotImplementedException();
                //_suspensionManager = _suspensionManager ?? Cross.IoCProvider.Resolve<ICrossSuspensionManager>();
                //return _suspensionManager;
            }
        }


        protected virtual ICrossBundle? LoadStateBundle(NavigationEventArgs e)
        {
            // nothing loaded by default
            var frameState = SuspensionManager.SessionStateForFrame(WrappedFrame);
            _pageKey = "Page-" + Frame.BackStackDepth;
            ICrossBundle? bundle = null;

            if (e.NavigationMode == NavigationMode.New)
            {
                // Clear existing state for forward navigation when adding a new page to the
                // navigation stack
                var nextPageKey = _pageKey;
                var nextPageIndex = Frame.BackStackDepth;
                while (frameState.Remove(nextPageKey))
                {
                    nextPageIndex++;
                    nextPageKey = "Page-" + nextPageIndex;
                }
            }
            else
            {
                var dictionary = (IDictionary<string, string>)frameState[_pageKey];
                bundle = new CrossBundle(dictionary);
            }

            return bundle;
        }

        protected virtual void SaveStateBundle(NavigationEventArgs navigationEventArgs, ICrossBundle bundle)
        {
            var frameState = SuspensionManager.SessionStateForFrame(WrappedFrame);
            frameState[_pageKey] = bundle.Data;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~CrossWindowsPage()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Loading -= CrossWindowsPage_Loading;
                Loaded -= CrossWindowsPage_Loaded;
                Unloaded -= CrossWindowsPage_Unloaded;
            }
        }
    }
}
