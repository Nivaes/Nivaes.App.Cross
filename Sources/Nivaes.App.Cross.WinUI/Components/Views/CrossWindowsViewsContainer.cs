using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.WinUI;

internal class CrossWindowsViewsContainer
    : ICrossStoreViewsContainer
{
    private const string ExtrasKey = "MvxLaunchData";
    private const string SubViewModelKey = "MvxSubViewModelKey";

    //private readonly ICrossViewModelLoader _viewModelLoader;
    private readonly ICrossNavigationSerializer _navigationSerializer;
    private readonly ICrossChildViewModelCache _childViewModelCache;
    private readonly ILogger _logger;

    public CrossWindowsViewsContainer(
        //ICrossViewModelLoader viewModelLoader,
        ICrossNavigationSerializer navigationSerializer,
        ICrossChildViewModelCache childViewModelCache,
        ILogger<CrossWindowsViewsContainer> logger)
    {
        //_viewModelLoader = viewModelLoader;
        _navigationSerializer = navigationSerializer;
        _childViewModelCache = childViewModelCache;
        _logger = logger;
    }

    public ICrossViewModel Load(string requestText, ICrossBundle savedState)
    {
        var dictionary = _navigationSerializer.Serializer.DeserializeObject<Dictionary<string, string>>(requestText);

        dictionary.TryGetValue(ExtrasKey, out string serializedRequest);
        var request = _navigationSerializer.Serializer.DeserializeObject<ViewModelRequest>(serializedRequest!);

        if (dictionary.TryGetValue(SubViewModelKey, out string? viewModelKey))
        {
            var key = int.Parse(viewModelKey);
            var viewModel = _childViewModelCache.Get(key);
            if (savedState != null)
                viewModel!.ReloadState(savedState);
            return viewModel!;
        }

        throw new AppException($"Not {SubViewModelKey} found.");
        //return _viewModelLoader.LoadViewModel(request!, savedState);
    }

    #region Implementation of IMvxWindowsViewModelRequestTranslator
    public string GetRequestTextFor(ViewModelRequest request)
    {
        var returnData = new Dictionary<string, string>();

        returnData.Add(ExtrasKey, _navigationSerializer!.Serializer.SerializeObject(request));

        var requestText = _navigationSerializer.Serializer.SerializeObject(returnData);
        return requestText;
    }

    public string GetRequestTextWithKeyFor(ICrossViewModel existingViewModelToUse)
    {
        var returnData = new Dictionary<string, string>();
        
        var request = ViewModelRequest.GetDefaultRequest(existingViewModelToUse.GetType());

        var key = _childViewModelCache.Cache(existingViewModelToUse);
        returnData.Add(ExtrasKey, _navigationSerializer.Serializer.SerializeObject(request));
        returnData.Add(SubViewModelKey, key!.ToString()!);

        var requestText = _navigationSerializer.Serializer.SerializeObject(returnData);

        return requestText;
    }

    public void RemoveSubViewModelWithKey(int key)
    {
        _childViewModelCache.Remove(key);
    }

    public int RequestTextGetKey(string requestText)
    {
        var returnValue = 0;

        var dictionary = _navigationSerializer!.Serializer.DeserializeObject<Dictionary<string, string>>(requestText);

        dictionary!.TryGetValue(ExtrasKey, out string? serializedRequest);
        var request = _navigationSerializer!.Serializer.DeserializeObject<ViewModelRequest>(serializedRequest!);

        if (dictionary.TryGetValue(SubViewModelKey, out string? viewModelKey))
        {
            returnValue = int.Parse(viewModelKey);
        }
        return returnValue;
    }
    #endregion Implementation of IMvxWindowsViewModelRequestTranslator
}
