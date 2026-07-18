using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Droid;

public class MvxSavedStateConverter : IMvxSavedStateConverter
{
    private const string ExtrasKey = "MvxSaved";
    private readonly ICrossNavigationSerializer _serializer;
    private readonly ILogger _logger;


    public MvxSavedStateConverter(ICrossNavigationSerializer serializer, ILogger<MvxSavedStateConverter> logger)
    {
        _serializer = serializer;
        _logger = logger;
    }

    public ICrossBundle? Read(Bundle? bundle)
    {
        var extras = bundle?.GetString(ExtrasKey);
        if (string.IsNullOrEmpty(extras))
            return null;

        try
        {
            var data = _serializer.Serializer.DeserializeObject<Dictionary<string, string>>(extras);
            return new CrossBundle(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problem getting the saved state - will return null - from {Extras}", extras);
            return null;
        }
    }

    public void Write(Bundle bundle, ICrossBundle? savedState)
    {
        if (savedState == null)
            return;

        if (savedState.Data.Count == 0)
            return;

        var data = _serializer.Serializer.SerializeObject(savedState.Data);
        bundle.PutString(ExtrasKey, data);
    }
}
