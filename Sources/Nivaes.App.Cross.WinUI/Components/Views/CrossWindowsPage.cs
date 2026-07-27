using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Windows.UI.Core;

namespace Nivaes.App.Cross.WinUI;

public abstract class CrossWindowsPage<TViewModel>
    : Page
    , IDisposable
    , ICrossWindowsView<TViewModel>
    , ICrossWindowsView
    where TViewModel : ICrossViewModel
{
    private TViewModel? _viewModel;

    public CrossWindowsPage()
    {
        Loading += WindowsPage_Loading;
        Loaded += WindowsPage_Loaded;
        Unloaded += WindowsPage_Unloaded;
    }

    private void WindowsPage_Loading(FrameworkElement sender, object args)
    {
        ViewModel?.ViewAppearing();
    }

    private void WindowsPage_Loaded(object sender, RoutedEventArgs e)
    {
        ViewModel?.ViewAppeared();
    }

    private void WindowsPage_Unloaded(object sender, RoutedEventArgs e)
    {
        ViewModel?.ViewDestroy();
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        ViewModel?.ViewDisappearing();
        base.OnNavigatingFrom(e);
    }



    public ICrossWindowsFrame WrappedFrame => new CrossWindowsFrame(Frame);

    public TViewModel? ViewModel
    {
        get
        {
            return _viewModel;
        }
        set
        {
            if (EqualityComparer<TViewModel>.Default.Equals(_viewModel, value))
                return;

            _viewModel = value;
            DataContext = ViewModel;
            OnViewModelSet();
        }
    }

    ICrossViewModel? ICrossView.ViewModel { get => ViewModel; set => ViewModel = (TViewModel?)value; }

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

        //UpdateBackButtonVisibility();
    }

    //protected virtual void UpdateBackButtonVisibility()
    //{
    //    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
    //}

    private byte[]? _reqData = null;

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        ViewModel?.ViewCreated();

        if (_reqData != null)
        {
            Debugger.Break(); // Mirar lo que carga aquí.
            //var viewModelLoader = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossWindowsViewModelLoader>();
            //ViewModel = (TViewModel?)viewModelLoader?.Load(e.Parameter.ToString(), LoadStateBundle(e));
            //ViewModel?.ViewCreated();
        }
        _reqData = (byte[])e.Parameter;

        this.OnViewCreate(_reqData, () => LoadStateBundle(e));
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        ViewModel?.ViewDisappeared();
        base.OnNavigatedFrom(e);
        var bundle = this.CreateSaveStateBundle();
        SaveStateBundle(e, bundle);

        //var translator = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossWindowsViewModelRequestTranslator>();

        if (e.NavigationMode == Microsoft.UI.Xaml.Navigation.NavigationMode.Back)
        {
            var idRequest = ViewModelRequestSerializer.DeserializeId(_reqData ?? new byte[0]);
            this.OnViewDestroy(idRequest);
        }
        else
        {
            var backstack = Frame.BackStack;
            if (backstack.Count > 0)
            {
                var currentEntry = backstack[backstack.Count - 1];
                var request = ViewModelRequestSerializer.Deserialize((byte[])currentEntry.Parameter);

                //ToDo: ¿Es necesario hacer una copia del Request?
                var newRequest = new ViewModelRequest(request.ViewModel);
                var newRequestBuffer = ViewModelRequestSerializer.Serializer(newRequest);
                var entry = new PageStackEntry(currentEntry.SourcePageType, newRequestBuffer, currentEntry.NavigationTransitionInfo);
                backstack.Remove(currentEntry);
                backstack.Add(entry);
            }
        }
    }

    private string? _pageKey;

    private ICrossSuspensionManager? _suspensionManager;

    protected ICrossSuspensionManager? SuspensionManager
    {
        // ToDo: Buscar la manera de guardar la sesión de otra manera, que no necesite inyección de dependencias. 
        // O buscar la manera de acceder al contenerdor de dependencias desde una vista.    
        get
        {
            _suspensionManager = _suspensionManager ?? IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossSuspensionManager>();
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
        if (_pageKey != null)
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
            Loading -= WindowsPage_Loading;
            Loaded -= WindowsPage_Loaded;
            Unloaded -= WindowsPage_Unloaded;
        }
    }
}
