namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using MvvmCross;
    using MvvmCross.Platforms.Android.Core;

    [RequiresUnreferencedCode("This class uses reflection which may not be preserved during trimming.")]
    public class MvxSavedStateConverter : IMvxSavedStateConverter
    {
        private const string ExtrasKey = "MvxSaved";

        public ICrossBundle? Read(Bundle bundle)
        {
            var extras = bundle?.GetString(ExtrasKey);
            if (string.IsNullOrEmpty(extras))
                return null;

            try
            {
                var converter = Mvx.IoCProvider.Resolve<ICrossNavigationSerializer>();
                var data = converter.Serializer.DeserializeObject<Dictionary<string, string>>(extras);
                return new CrossBundle(data);
            }
            catch (Exception ex)
            {
                CrossLogHost.Default?.Log(LogLevel.Error, ex,
                    "Problem getting the saved state - will return null - from {Extras}", extras);
                return null;
            }
        }

        public void Write(Bundle bundle, ICrossBundle savedState)
        {
            if (savedState == null)
                return;

            if (savedState.Data.Count == 0)
                return;

            var converter = Mvx.IoCProvider.Resolve<ICrossNavigationSerializer>();
            var data = converter.Serializer.SerializeObject(savedState.Data);
            bundle.PutString(ExtrasKey, data);
        }
    }
}
