using Android.Content;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Droid;

public sealed class AndroidViewsContainer
    : CrossViewsContainer, IAndroidViewsContainer
{
    private const string ExtrasKey = "LaunchDataKey";
    private const string SubViewModelKey = "SubViewModelKey";

    private readonly Context _applicationContext;
    private readonly ICrossNavigationSerializer _navigationSerializer;
    private readonly ICrossChildViewModelCache _childViewModelCache;

    public AndroidViewsContainer(Context applicationContext,
        ICrossNavigationSerializer navigationSerializer, ICrossChildViewModelCache childViewModelCache,
        ILogger<AndroidViewsContainer> logger)
        : base(logger)
    {
        _applicationContext = applicationContext;
        _navigationSerializer = navigationSerializer;
        _childViewModelCache = childViewModelCache;
    }

    #region Implementation of IMvxAndroidViewModelRequestTranslator

    public ICrossViewModel? Load(Intent intent, ICrossBundle? savedState)
    {
        return Load(intent, null, null);
    }

    public ICrossViewModel? Load(Intent intent, ICrossBundle? savedState, Type? viewModelTypeHint)
    {
        return CreateViewModel(intent, savedState, viewModelTypeHint);
    }

    private ICrossViewModel? CreateViewModel(
        Intent intent,
        ICrossBundle? savedState,
        Type? viewModelTypeHint)
    {
        ArgumentNullException.ThrowIfNull(intent);

        if (TryGetEmbeddedViewModel(intent, out var mvxViewModel))
        {
            base.Logger.Log(LogLevel.Trace, "Embedded ViewModel used");
            return mvxViewModel;
        }

        Logger.Log(LogLevel.Trace, "Attempting to load new ViewModel from Intent with Extras");
        var toReturn = CreateViewModelFromIntent(intent, savedState);
        if (toReturn != null)
            return toReturn;

        Logger?.Log(LogLevel.Trace, "ViewModel not loaded from Extras - will try DirectLoad");
        return DirectLoad(savedState, viewModelTypeHint);
    }

    private ICrossViewModel? DirectLoad(
        ICrossBundle? savedState,
        Type? viewModelTypeHint)
    {
        if (viewModelTypeHint == null)
        {
            throw new AppException("Unable to load viewmodel - no type hint provided");
        }

        var viewModelLoader = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossViewModelLoader>();

        var viewModelRequest = CrossViewModelRequest.GetDefaultRequest(viewModelTypeHint);
        var viewModel = viewModelLoader.LoadViewModel(viewModelRequest, savedState);
        return viewModel;
    }

    private ICrossViewModel? CreateViewModelFromIntent(Intent intent, ICrossBundle? savedState)
    {
        var extraData = intent.Extras?.GetString(ExtrasKey);
        if (extraData == null)
            return null;

        var viewModelRequest = _navigationSerializer.Serializer.DeserializeObject<CrossViewModelRequest>(extraData);
        return ViewModelFromRequest(viewModelRequest, savedState);
    }

    private ICrossViewModel? ViewModelFromRequest(CrossViewModelRequest? viewModelRequest, ICrossBundle? savedState)
    {
        if (viewModelRequest == null)
            return null;

        var viewModelLoader = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossViewModelLoader>();

        return viewModelLoader.LoadViewModel(viewModelRequest, savedState);
    }

    private bool TryGetEmbeddedViewModel(Intent intent, out ICrossViewModel? mvxViewModel)
    {
        var embeddedViewModelKey = intent.Extras?.GetInt(SubViewModelKey);
        if (embeddedViewModelKey != null && embeddedViewModelKey.Value != 0)
        {
            mvxViewModel = _childViewModelCache.Get(embeddedViewModelKey.Value);
            if (mvxViewModel != null)
            {
                RemoveSubViewModelWithKey(embeddedViewModelKey.Value);
                return true;
            }
        }

        mvxViewModel = null;
        return false;
    }

    public Intent GetIntentFor(CrossViewModelRequest request)
    {
        var viewType = GetViewType(request.ViewModelType!);
        if (viewType == null)
        {
            throw new AppException("View Type not found for " + request.ViewModelType);
        }

        var intent = new Intent(_applicationContext, viewType);

        //if (Mvx.IoCProvider?.TryResolve(out ICrossNavigationSerializer? navigationSerializer) != true ||
        //    navigationSerializer == null)
        //{
        //    return intent;
        //}

        var requestText = _navigationSerializer.Serializer.SerializeObject(request);
        intent.PutExtra(ExtrasKey, requestText);
        AdjustIntentForPresentation(intent, request);

        return intent;
    }

    private void AdjustIntentForPresentation(Intent intent, CrossViewModelRequest request)
    {
        //todo we want to do things here... clear top, remove history item, etc
        //#warning ClearTop is not enough :/ Need to work on an Intent based scheme like http://stackoverflow.com/questions/3007998/on-logout-clear-activity-history-stack-preventing-back-button-from-opening-l
        //            if (request.ClearTop)
        //                intent.AddFlags(ActivityFlags.ClearTop);
    }

    public (Intent intent, int key) GetIntentWithKeyFor<TViewModel>(
            TViewModel existingViewModelToUse,
            CrossViewModelRequest? request)
        where TViewModel : ICrossViewModel
    {
        request ??= CrossViewModelRequest.GetDefaultRequest(existingViewModelToUse.GetType());
        var intent = GetIntentFor(request);

        //if (Mvx.IoCProvider?.TryResolve(out ICrossChildViewModelCache? viewModelCache) != true || viewModelCache == null)
        //{
        //    return (intent, -1);
        //}

        var key = _childViewModelCache.Cache(existingViewModelToUse);
        intent.PutExtra(SubViewModelKey, key);
        return (intent, key);
    }

    public void RemoveSubViewModelWithKey(int key)
    {
        //if (Mvx.IoCProvider?.TryResolve(out ICrossChildViewModelCache? viewModelCache) == true && viewModelCache != null)
        //{
        _childViewModelCache.Remove(key);
        //}
    }

    #endregion Implementation of IMvxAndroidViewModelRequestTranslator
}