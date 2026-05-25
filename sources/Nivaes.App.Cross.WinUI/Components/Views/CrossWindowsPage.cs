using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Nivaes.IoC;
using Windows.UI.Core;

namespace Nivaes.App.Cross.WinUI;

public class CrossWindowsPage<TViewModel>
    : Page
    , IDisposable
    , ICrossWindowsView<TViewModel> 
    , ICrossWindowsView
    where TViewModel : class, ICrossViewModel
{
    private TViewModel? _viewModel;

    public CrossWindowsPage()
    {
        Loading += MvxWindowsPage_Loading;
        Loaded += MvxWindowsPage_Loaded;
        Unloaded += MvxWindowsPage_Unloaded;
    }

    private void MvxWindowsPage_Loading(FrameworkElement sender, object args)
    {
        ViewModel?.ViewAppearing();
    }

    private void MvxWindowsPage_Loaded(object sender, RoutedEventArgs e)
    {
        ViewModel?.ViewAppeared();
    }

    private void MvxWindowsPage_Unloaded(object sender, RoutedEventArgs e)
    {
        ViewModel?.ViewDestroy();
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        ViewModel?.ViewDisappearing();
        base.OnNavigatingFrom(e);
    }

    

    public ICrossWindowsFrame WrappedFrame => new CrossWrappedFrame(Frame);

    ICrossViewModel? ICrossView.ViewModel 
    { 
        get => ViewModel; 
        set => ViewModel = (TViewModel?)value;
    }

    public TViewModel? ViewModel
    {
        get
        {
            return _viewModel;
        }
        set
        {
            if (_viewModel == value)
                return;

            _viewModel = value;
            DataContext = ViewModel;
            OnViewModelSet();
        }
    }

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
            var viewModelLoader = Mvx.IoCProvider.Resolve<ICrossWindowsViewModelLoader>();
            ViewModel = (TViewModel?)viewModelLoader?.Load(e.Parameter.ToString(), LoadStateBundle(e));
            ViewModel?.ViewCreated();
        }
        _reqData = (string)e.Parameter;

        this.OnViewCreate(_reqData, () => LoadStateBundle(e));
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        ViewModel?.ViewDisappeared();
        base.OnNavigatedFrom(e);
        var bundle = this.CreateSaveStateBundle();
        SaveStateBundle(e, bundle);

        var translator = Mvx.IoCProvider.Resolve<ICrossWindowsViewModelRequestTranslator>();

        if (e.NavigationMode == Microsoft.UI.Xaml.Navigation.NavigationMode.Back)
        {
            var key = translator.RequestTextGetKey(_reqData);
            this.OnViewDestroy(key);
        }
        else
        {
            var backstack = Frame.BackStack;
            if (backstack.Count > 0)
            {
                var currentEntry = backstack[backstack.Count - 1];
                var key = translator.RequestTextGetKey(currentEntry.Parameter.ToString());
                if (key == 0)
                {
                    var newParamter = translator.GetRequestTextWithKeyFor(ViewModel);
                    var entry = new PageStackEntry(currentEntry.SourcePageType, newParamter, currentEntry.NavigationTransitionInfo);
                    backstack.Remove(currentEntry);
                    backstack.Add(entry);
                }
            }
        }
    }

    private string? _pageKey;

    private ICrossSuspensionManager? _suspensionManager;
    protected ICrossSuspensionManager? SuspensionManager
    {
        get
        {
            _suspensionManager = _suspensionManager ?? Mvx.IoCProvider.Resolve<ICrossSuspensionManager>();
            return _suspensionManager;
        }
    }

    protected virtual ICrossBundle? LoadStateBundle(NavigationEventArgs e)
    {
        // nothing loaded by default
        var frameState = SuspensionManager?.SessionStateForFrame(WrappedFrame);
        _pageKey = "Page-" + Frame.BackStackDepth;
        ICrossBundle? bundle = null;

        if (e.NavigationMode == Microsoft.UI.Xaml.Navigation.NavigationMode.New)
        {
            // Clear existing state for forward navigation when adding a new page to the
            // navigation stack
            var nextPageKey = _pageKey;
            var nextPageIndex = Frame.BackStackDepth;
            if (frameState != null)
            {
                while (frameState.Remove(nextPageKey))
                {
                    nextPageIndex++;
                    nextPageKey = "Page-" + nextPageIndex;
                }
            }
        }
        else
        {
            var dictionary = (IDictionary<string, string>?)frameState?[_pageKey];
            bundle = new CrossBundle(dictionary);
        }

        return bundle;
    }

    protected virtual void SaveStateBundle(NavigationEventArgs navigationEventArgs, ICrossBundle bundle)
    {
        var frameState = SuspensionManager?.SessionStateForFrame(WrappedFrame);
        if(_pageKey != null)
            frameState?[_pageKey] = bundle.Data;
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
            Loading -= MvxWindowsPage_Loading;
            Loaded -= MvxWindowsPage_Loaded;
            Unloaded -= MvxWindowsPage_Unloaded;
        }
    }
}
