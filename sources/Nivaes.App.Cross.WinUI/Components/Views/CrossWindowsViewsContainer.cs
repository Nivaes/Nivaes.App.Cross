using ABI.System;
using Microsoft.Extensions.DependencyInjection;
using Nivaes.App.Cross;
using Nivaes.IoC;

namespace Nivaes.App.Cross.WinUI;

internal class CrossWindowsViewsContainer
    : CrossViewsContainer
    , ICrossStoreViewsContainer
{
    private const string ExtrasKey = "MvxLaunchData";
    private const string SubViewModelKey = "MvxSubViewModelKey";

    private IServiceProvider _serviceProvider;

    public CrossWindowsViewsContainer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ICrossViewModel Load(string requestText, ICrossBundle savedState)
    {
        var converter = _serviceProvider.GetService<ICrossNavigationSerializer>();
        var dictionary = converter.Serializer.DeserializeObject<Dictionary<string, string>>(requestText);

        dictionary.TryGetValue(ExtrasKey, out string serializedRequest);
        var request = converter.Serializer.DeserializeObject<CrossViewModelRequest>(serializedRequest!);

        if (dictionary.TryGetValue(SubViewModelKey, out string? viewModelKey))
        {
            var key = int.Parse(viewModelKey);
            var viewModel = Mvx.IoCProvider.Resolve<ICrossChildViewModelCache>()!.Get(key);
            if (savedState != null)
                viewModel!.ReloadState(savedState);
            return viewModel!;
        }

        var loaderService = _serviceProvider.GetService<ICrossViewModelLoader>();
        return loaderService!.LoadViewModel(request!, savedState);
    }

    #region Implementation of IMvxWindowsViewModelRequestTranslator
    public string GetRequestTextFor(CrossViewModelRequest request)
    {
        var returnData = new Dictionary<string, string>();
        var converter = _serviceProvider.GetService<ICrossNavigationSerializer>();

        returnData.Add(ExtrasKey, converter!.Serializer.SerializeObject(request));

        var requestText = converter.Serializer.SerializeObject(returnData);
        return requestText;
    }

    public string GetRequestTextWithKeyFor(ICrossViewModel existingViewModelToUse)
    {
        var returnData = new Dictionary<string, string>();
        var converter = _serviceProvider.GetService<ICrossNavigationSerializer>();
        var request = CrossViewModelRequest.GetDefaultRequest(existingViewModelToUse.GetType());

        var key = Mvx.IoCProvider.Resolve<ICrossChildViewModelCache>()!.Cache(existingViewModelToUse);
        returnData.Add(ExtrasKey, converter!.Serializer!.SerializeObject(request));
        returnData.Add(SubViewModelKey, key!.ToString()!);

        var requestText = converter.Serializer.SerializeObject(returnData);

        return requestText;
    }

    public void RemoveSubViewModelWithKey(int key)
    {
        var cache = _serviceProvider.GetService<ICrossChildViewModelCache>();
        cache?.Remove(key);
    }

    public int RequestTextGetKey(string requestText)
    {
        var returnValue = 0;
        var converter = _serviceProvider.GetService<ICrossNavigationSerializer>();

        var dictionary = converter!.Serializer.DeserializeObject<Dictionary<string, string>>(requestText);

        dictionary!.TryGetValue(ExtrasKey, out string? serializedRequest);
        var request = converter!.Serializer.DeserializeObject<CrossViewModelRequest>(serializedRequest!);

        if (dictionary.TryGetValue(SubViewModelKey, out string? viewModelKey))
        {
            returnValue = int.Parse(viewModelKey);
        }
        return returnValue;
    }
    #endregion Implementation of IMvxWindowsViewModelRequestTranslator
}
