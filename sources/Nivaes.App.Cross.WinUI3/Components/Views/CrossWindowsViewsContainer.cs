namespace Nivaes.App.Cross.WinUI3
{
    using System.Collections.Generic;

    public class CrossWindowsViewsContainer
        : CrossViewsContainer
        , ICrossStoreViewsContainer
    {
        private const string ExtrasKey = "CrossLaunchData";
        private const string SubViewModelKey = "CrossSubViewModelKey";

        public ICrossViewModel Load(string requestText, ICrossBundle savedState)
        {
            throw new NotImplementedException();
            //var converter = Cross.IoCProvider.Resolve<CrossNavigationSerializer>();
            //var dictionary = converter.Serializer.DeserializeObject<Dictionary<string, string>>(requestText);

            //dictionary.TryGetValue(ExtrasKey, out string serializedRequest);
            //var request = converter.Serializer.DeserializeObject<CrossViewModelRequest>(serializedRequest);

            //if (dictionary.TryGetValue(SubViewModelKey, out string viewModelKey))
            //{
            //    var key = int.Parse(viewModelKey);
            //    var viewModel = Cross.IoCProvider.Resolve<ICrossChildViewModelCache>().Get(key);
            //    if (savedState != null)
            //        viewModel.ReloadState(savedState);
            //    return viewModel;
            //}

            //var loaderService = Cross.IoCProvider.Resolve<ICrossViewModelLoader>();
            //return loaderService.LoadViewModel(request, savedState);
        }

        #region Implementation of ICrossWindowsViewModelRequestTranslator
        //public string GetRequestTextFor(CrossViewModelRequest request)
        //{
        //    throw new NotImplementedException();

        //    //var returnData = new Dictionary<string, string>();
        //    //var converter = Cross.IoCProvider.Resolve<ICrossNavigationSerializer>();

        //    //returnData.Add(ExtrasKey, converter.Serializer.SerializeObject(request));

        //    //var requestText = converter.Serializer.SerializeObject(returnData);
        //    //return requestText;
        //}

        public string GetRequestTextWithKeyFor(ICrossViewModel existingViewModelToUse)
        {
            throw new NotImplementedException();

            //var returnData = new Dictionary<string, string>();
            //var converter = Cross.IoCProvider.Resolve<ICrossNavigationSerializer>();
            //var request = CrossViewModelRequest.GetDefaultRequest(existingViewModelToUse.GetType());

            //var key = Cross.IoCProvider.Resolve<ICrossChildViewModelCache>().Cache(existingViewModelToUse);
            //returnData.Add(ExtrasKey, converter.Serializer.SerializeObject(request));
            //returnData.Add(SubViewModelKey, key.ToString());

            //var requestText = converter.Serializer.SerializeObject(returnData);

            //return requestText;
        }

        public void RemoveSubViewModelWithKey(int key)
        {
            throw new NotImplementedException();
            //Cross.IoCProvider.Resolve<ICrossChildViewModelCache>().Remove(key);
        }

        public int RequestTextGetKey(string requestText)
        {
            throw new NotImplementedException();

            //var returnValue = 0;
            //var converter = Cross.IoCProvider.Resolve<ICrossNavigationSerializer>();
            //var dictionary = converter.Serializer.DeserializeObject<Dictionary<string, string>>(requestText);

            //dictionary.TryGetValue(ExtrasKey, out string serializedRequest);
            //var request = converter.Serializer.DeserializeObject<CrossViewModelRequest>(serializedRequest);

            //if (dictionary.TryGetValue(SubViewModelKey, out string viewModelKey))
            //{
            //    returnValue = int.Parse(viewModelKey);
            //}
            //return returnValue;
        }
        #endregion Implementation of ICrossWindowsViewModelRequestTranslator
    }
}
