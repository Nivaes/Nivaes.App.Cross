namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;

    [RequiresUnreferencedCode("This class uses reflection which may not be preserved during trimming.")]
    public class CrossSavedStateConverter : ICrossSavedStateConverter
    {
        private const string ExtrasKey = "MvxSaved";

        public ICrossBundle Read(Bundle bundle)
        {
            var extras = bundle?.GetString(ExtrasKey);
            if (string.IsNullOrEmpty(extras))
                return null;

            throw new NotImplementedException();
            //try
            //{
            //    var converter = Mvx.IoCProvider.Resolve<IMvxNavigationSerializer>();
            //    var data = converter.Serializer.DeserializeObject<Dictionary<string, string>>(extras);
            //    return new MvxBundle(data);
            //}
            //catch (Exception ex)
            //{
            //    CrossLogHost.Default?.Log(LogLevel.Error, ex,
            //        "Problem getting the saved state - will return null - from {Extras}", extras);
            //    return null;
            //}
        }

        public void Write(Bundle bundle, ICrossBundle savedState)
        {
            if (savedState == null)
                return;

            if (savedState.Data.Count == 0)
                return;

            throw new NotImplementedException();
            //var converter = Mvx.IoCProvider.Resolve<IMvxNavigationSerializer>();
            //var data = converter.Serializer.SerializeObject(savedState.Data);
            //bundle.PutString(ExtrasKey, data);
        }
    }
}
