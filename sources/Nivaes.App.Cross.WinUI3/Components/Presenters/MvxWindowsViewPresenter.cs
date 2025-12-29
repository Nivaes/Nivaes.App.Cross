using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Nivaes.IoC;
using Windows.UI.Core;
using Control = Microsoft.UI.Xaml.Controls.Control;

namespace Nivaes.App.Cross.WinUI3;

public class MvxWindowsViewPresenter
    : CrossAttributeViewPresenter, IMvxWindowsViewPresenter
{
    protected readonly ICrossWindowsFrame _rootFrame;
    private readonly ILogger<MvxWindowsViewPresenter> _logger;

    public MvxWindowsViewPresenter(ICrossWindowsFrame rootFrame)
    {
        _rootFrame = rootFrame;
        _logger = CrossLogHost.GetLog<MvxWindowsViewPresenter>();

        if (Window.Current != null)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += BackButtonOnBackRequested;
        }
    }

    private ICrossViewModelLoader _viewModelLoader;
    public ICrossViewModelLoader ViewModelLoader
    {
        get
        {
            if (_viewModelLoader == null)
                _viewModelLoader = Mvx.IoCProvider.Resolve<ICrossViewModelLoader>();
            return _viewModelLoader;
        }
        set
        {
            _viewModelLoader = value;
        }
    }

    public override void RegisterAttributeTypes()
    {
        AttributeTypesToActionsDictionary.Register<MvxPagePresentationAttribute>(ShowPage, ClosePage);
        AttributeTypesToActionsDictionary.Register<MvxSplitViewPresentationAttribute>(ShowSplitView, CloseSplitView);
        AttributeTypesToActionsDictionary.Register<MvxRegionPresentationAttribute>(ShowRegionView, CloseRegionView);
        AttributeTypesToActionsDictionary.Register<MvxDialogViewPresentationAttribute>(ShowDialog, CloseDialog);
    }

    public override CrossBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
    {
        _logger?.LogTrace("PresentationAttribute not found for {viewTypeName}. Assuming new page presentation", viewType.Name);
        return new MvxPagePresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
    }

    protected virtual async void BackButtonOnBackRequested(object sender, BackRequestedEventArgs backRequestedEventArgs)
    {
        if (backRequestedEventArgs.Handled)
            return;

        var currentView = _rootFrame.Content as ICrossView;
        if (currentView == null)
        {
            _logger?.LogWarning("Ignoring close for viewmodel - rootframe has no current page");
            return;
        }

        var navigationService = Mvx.IoCProvider.Resolve<ICrossNavigationService>();

        backRequestedEventArgs.Handled = await navigationService.Close(currentView.ViewModel);
    }

    protected virtual string GetRequestText(CrossViewModelRequest request)
    {
        var requestTranslator = Mvx.IoCProvider.Resolve<ICrossWindowsViewModelRequestTranslator>();
        string requestText = string.Empty;
        if (request is CrossViewModelInstanceRequest)
        {
            requestText = requestTranslator.GetRequestTextWithKeyFor(((CrossViewModelInstanceRequest)request).ViewModelInstance);
        }
        else
        {
            requestText = requestTranslator.GetRequestTextFor(request);
        }

        return requestText;
    }

    protected virtual void HandleBackButtonVisibility()
    {
        if (Window.Current == null)
            return;

        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
            _rootFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
    }

    protected virtual Task<bool> ShowSplitView(Type viewType, MvxSplitViewPresentationAttribute attribute, CrossViewModelRequest request)
    {
        var viewsContainer = Mvx.IoCProvider.Resolve<ICrossViewsContainer>();

        if (_rootFrame.Content is ICrossWindowsView currentPage)
        {
            var splitView = currentPage.Content.FindControl<SplitView>();
            if (splitView == null)
            {
                return Task.FromResult(true);
            }

            if (attribute.Position == SplitPanePosition.Content)
            {
                var nestedFrame = splitView.Content as Frame;
                if (nestedFrame == null)
                {
                    nestedFrame = new Frame();
                    splitView.Content = nestedFrame;
                }
                var requestText = GetRequestText(request);
                nestedFrame.Navigate(viewType, requestText);
            }
            else if (attribute.Position == SplitPanePosition.Pane)
            {
                var nestedFrame = splitView.Pane as Frame;
                if (nestedFrame == null)
                {
                    nestedFrame = new Frame();
                    splitView.Pane = nestedFrame;
                }
                var requestText = GetRequestText(request);
                nestedFrame.Navigate(viewType, requestText);
            }
        }
        return Task.FromResult(true);
    }

    protected virtual Task<bool> CloseSplitView(ICrossViewModel viewModel, MvxSplitViewPresentationAttribute attribute)
    {
        return ClosePage(viewModel, attribute);
    }

    protected virtual Task<bool> ShowRegionView(Type viewType, MvxRegionPresentationAttribute attribute, CrossViewModelRequest request)
    {
        if (viewType.HasRegionAttribute())
        {
            var requestText = GetRequestText(request);

            var containerView = _rootFrame.UnderlyingControl.FindControl<Frame>(viewType.GetRegionName());

            if (containerView != null)
            {
                containerView.Navigate(viewType, requestText);
                return Task.FromResult(true);
            }
        }
        return Task.FromResult(true);
    }

    protected virtual Task<bool> CloseRegionView(ICrossViewModel viewModel, MvxRegionPresentationAttribute attribute)
    {
        var viewFinder = Mvx.IoCProvider.Resolve<ICrossViewsContainer>();
        var viewType = viewFinder.GetViewType(viewModel.GetType());
        if (viewType.HasRegionAttribute())
        {
            var containerView = _rootFrame.UnderlyingControl?.FindControl<Frame>(viewType.GetRegionName());

            if (containerView == null)
                throw new CrossException($"Region '{viewType.GetRegionName()}' not found in view '{viewType}'");

            if (containerView.CanGoBack)
            {
                containerView.GoBack();
                return Task.FromResult(true);
            }
        }

        return ClosePage(viewModel, attribute);
    }

    protected virtual Task<bool> ClosePage(ICrossViewModel viewModel, CrossBasePresentationAttribute attribute)
    {
        var currentView = _rootFrame.Content as ICrossView;
        if (currentView == null)
        {
            _logger?.LogWarning("Ignoring close for viewmodel - rootframe has no current page");
            return Task.FromResult(false);
        }

        if (currentView.ViewModel != viewModel)
        {
            _logger?.LogWarning("Ignoring close for viewmodel - rootframe's current page is not the view for the requested viewmodel");
            return Task.FromResult(false);
        }

        if (!_rootFrame.CanGoBack)
        {
            _logger?.LogWarning("Ignoring close for viewmodel - rootframe refuses to go back");
            return Task.FromResult(false);
        }

        _rootFrame.GoBack();

        HandleBackButtonVisibility();

        return Task.FromResult(true);
    }

    protected virtual Task<bool> ShowPage(Type viewType, CrossBasePresentationAttribute attribute, CrossViewModelRequest request)
    {
        try
        {
            var requestText = GetRequestText(request);
            var viewsContainer = Mvx.IoCProvider.Resolve<ICrossViewsContainer>();

            _rootFrame.Navigate(viewType, requestText); //Frame won't allow serialization of it's nav-state if it gets a non-simple type as a nav param

            HandleBackButtonVisibility();
            return Task.FromResult(true);
        }
        catch (Exception exception)
        {
            _logger?.LogTrace(exception, "Error seen during navigation request to {viewModelTypeName}", request.ViewModelType.Name);
            return Task.FromResult(false);
        }
    }

    protected virtual async Task<bool> ShowDialog(Type viewType, MvxDialogViewPresentationAttribute attribute, CrossViewModelRequest request)
    {
        try
        {
            var contentDialog = (ContentDialog)CreateControl(viewType, request, attribute);

            if (_rootFrame.UnderlyingControl is Frame frame)
            {
                contentDialog.XamlRoot = frame.XamlRoot;
            }

            if (contentDialog != null)
            {
                await contentDialog.ShowAsync(attribute.Placement);
                return true;
            }

            return false;
        }
        catch (Exception exception)
        {
            _logger?.LogTrace(exception, "Error seen during navigation request to {viewModelTypeName}", request.ViewModelType.Name);
            return false;
        }
    }

    public virtual Control CreateControl(Type viewType, CrossViewModelRequest request, CrossBasePresentationAttribute attribute)
    {
        try
        {
            var control = Activator.CreateInstance(viewType) as Control;
            if (control is ICrossView mvxControl)
            {
                if (request is CrossViewModelInstanceRequest instanceRequest)
                    mvxControl.ViewModel = instanceRequest.ViewModelInstance;
                else
                    mvxControl.ViewModel = ViewModelLoader.LoadViewModel(request, null);
            }

            return control;
        }
        catch (Exception ex)
        {
            throw new CrossException(ex, $"Cannot create Control '{viewType.FullName}'. Are you use the wrong base class?");
        }
    }

    protected virtual Task<bool> CloseDialog(ICrossViewModel viewModel, CrossBasePresentationAttribute attribute)
    {
        if (!(_rootFrame.UnderlyingControl is Frame frame))
            return Task.FromResult(false);

        var popups = VisualTreeHelper.GetOpenPopupsForXamlRoot(frame.XamlRoot).FirstOrDefault(p =>
        {
            if (attribute.ViewType.IsInstanceOfType(p.Child)
                && p.Child is ICrossWindowsContentDialog dialog)
            {
                return dialog.ViewModel == viewModel;
            }
            return false;
        });

        (popups?.Child as ContentDialog)?.Hide();

        return Task.FromResult(true);
    }
}