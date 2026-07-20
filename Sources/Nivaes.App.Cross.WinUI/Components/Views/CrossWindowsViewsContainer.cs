using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.WinUI;

internal class CrossWindowsViewsContainer
    : ICrossStoreViewsContainer
{
    private const string ExtrasKey = "MvxLaunchData";
    private const string SubViewModelKey = "MvxSubViewModelKey";

    //private readonly ICrossViewModelLoader _viewModelLoader;
    //private readonly ICrossNavigationSerializer _navigationSerializer;
    //private readonly ICrossChildViewModelCache _childViewModelCache;
    private readonly ILogger _logger;

    public CrossWindowsViewsContainer(
        //ICrossViewModelLoader viewModelLoader,
        //ICrossNavigationSerializer navigationSerializer,
        //ICrossChildViewModelCache childViewModelCache,
        ILogger<CrossWindowsViewsContainer> logger)
    {
        //_viewModelLoader = viewModelLoader;
        //_navigationSerializer = navigationSerializer;
        //_childViewModelCache = childViewModelCache;
        _logger = logger;
    }

    public ICrossViewModel Load(byte[] requestBuffer, ICrossBundle savedState)
    {
        var request = ViewModelRequestSerializer.Deserialize(requestBuffer);

        if (savedState != null)
            request.ViewModel.ReloadState(savedState);

        return request.ViewModel;
    }

    #region Implementation of IMvxWindowsViewModelRequestTranslator
    [Obsolete("", true)]
    public string GetRequestTextFor(ViewModelRequest request)
    {
        throw new NotImplementedException("Obsolete");
        //var returnData = new Dictionary<string, string>();

        //returnData.Add(ExtrasKey, _navigationSerializer!.Serializer.SerializeObject(request));

        //var requestText = _navigationSerializer.Serializer.SerializeObject(returnData);
        //return requestText;
    }

    [Obsolete("", true)]
    public string GetRequestTextWithKeyFor(ICrossViewModel existingViewModelToUse)
    {
        throw new NotImplementedException("Obsolete");
        //var returnData = new Dictionary<string, string>();

        //var request = ViewModelRequest.GetDefaultRequest(existingViewModelToUse.GetType());

        //var key = _childViewModelCache.Cache(existingViewModelToUse);
        //returnData.Add(ExtrasKey, _navigationSerializer.Serializer.SerializeObject(request));
        //returnData.Add(SubViewModelKey, key!.ToString()!);

        //var requestText = _navigationSerializer.Serializer.SerializeObject(returnData);

        //return requestText;
    }

    [Obsolete("", true)]
    public void RemoveSubViewModelWithKey(int key)
    {
        throw new NotImplementedException("Obsolete");
        //_childViewModelCache.Remove(key);
    }

    [Obsolete("", true)]
    public int RequestTextGetKey(string requestText)
    {
        throw new NotImplementedException("Obsolete");
        //var returnValue = 0;

        //var dictionary = _navigationSerializer!.Serializer.DeserializeObject<Dictionary<string, string>>(requestText);

        //dictionary!.TryGetValue(ExtrasKey, out string? serializedRequest);
        //var request = _navigationSerializer!.Serializer.DeserializeObject<ViewModelRequest>(serializedRequest!);

        //if (dictionary.TryGetValue(SubViewModelKey, out string? viewModelKey))
        //{
        //    returnValue = int.Parse(viewModelKey);
        //}
        //return returnValue;
    }
    #endregion Implementation of IMvxWindowsViewModelRequestTranslator
}
