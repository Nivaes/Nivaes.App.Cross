using Android.Content;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Droid;

internal sealed class AndroidViewsContainer
    : IAndroidViewsContainer
{
    private const string ExtrasKey = "_LaunchDataKey";
    private const string SubViewModelKey = "_ViewModelId";

    private readonly Context _applicationContext;

    private readonly ILogger Logger;

    public AndroidViewsContainer(Context applicationContext,
        ILogger<AndroidViewsContainer> logger)
    {
        _applicationContext = applicationContext;
        Logger = logger;
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
            Logger.Log(LogLevel.Trace, "Embedded ViewModel used");
            return mvxViewModel;
        }

        Logger.Log(LogLevel.Trace, "Attempting to load new ViewModel from Intent with Extras");
        var toReturn = CreateViewModelFromIntent(intent, savedState);
        if (toReturn != null)
            return toReturn;

        Logger?.Log(LogLevel.Trace, "ViewModel not loaded from Extras - will try DirectLoad");
        return DirectLoad(savedState, viewModelTypeHint);
    }

    private ICrossViewModel DirectLoad(
        ICrossBundle? savedState,
        Type? viewModelTypeHint)
    {
        if (viewModelTypeHint == null)
        {
            throw new AppException("Unable to load viewmodel - no type hint provided");
        }

        var viewModelLoader = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<CrossViewModelLoader>();
        return viewModelLoader.LoadViewModel(viewModelTypeHint, null, savedState);
    }

    private ICrossViewModel? CreateViewModelFromIntent(Intent intent, ICrossBundle? savedState)
    {
        var extraData = intent.Extras?.GetByteArray(ExtrasKey);
        if (extraData == null)
            return null;

        var request = ViewModelRequestSerializer.Deserialize(extraData);

        return ViewModelFromRequest(request, savedState);
    }

    private ICrossViewModel? ViewModelFromRequest(IViewModelRequest? request, ICrossBundle? savedState)
    {
        if (request == null)
            return null;

        var viewModelLoader = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<CrossViewModelLoader>();
        return viewModelLoader.LoadViewModel(request, savedState);
    }

    private bool TryGetEmbeddedViewModel(Intent intent, out ICrossViewModel? viewModel)
    {
        var requestBuffer = intent.Extras?.GetByteArray(SubViewModelKey);
        if(requestBuffer != null)
        {
            var request = ViewModelRequestSerializer.Deserialize(requestBuffer, out var id);
            viewModel = request.ViewModel;
            ViewModelRequestCache.Delete(id);
        }

        viewModel = null;
        return false;
    }

    public Intent GetIntentFor(IViewModelRequest request)
    {
        var viewType = Singleton<ViewsContainers>.Instance.ViewModelViews[request.ViewModelType];

        var intent = new Intent(_applicationContext, viewType);

        var requestBuffer = ViewModelRequestSerializer.Serializer(request);
        intent.PutExtra(ExtrasKey, requestBuffer);

        return intent;
    }

    public (Intent intent, uint requestId) GetIntentWithKeyFor<TViewModel>(
            TViewModel existingViewModelToUse,
            IViewModelRequest? request)
        where TViewModel : ICrossViewModel
    {
        request ??= ViewModelRequest.GetDefaultRequest(existingViewModelToUse.GetType());
        var intent = GetIntentFor(request);

        ViewModelRequestCache.TryGetValue(existingViewModelToUse, out var requestId);

        intent.PutExtra(SubViewModelKey, (int)requestId!);
        return (intent, requestId ?? 0);
    }

    public void RemoveSubViewModelWithKey(uint requestId)
    {
        ViewModelRequestCache.Delete(requestId);
    }
    #endregion Implementation of IMvxAndroidViewModelRequestTranslator
}