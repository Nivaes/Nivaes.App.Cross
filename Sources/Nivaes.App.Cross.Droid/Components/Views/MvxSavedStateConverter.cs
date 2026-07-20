using System.Diagnostics.CodeAnalysis;
using System.Text;
using Java.Nio;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Droid;

public class MvxSavedStateConverter : IMvxSavedStateConverter
{
    private const string ExtrasKey = "_Saved";
    private readonly ILogger _logger;

    public MvxSavedStateConverter(ILogger<MvxSavedStateConverter> logger)
    {
        _logger = logger;
    }

    public ICrossBundle? Read(Bundle? bundle)
    {
        var extras = bundle?.GetByteArray(ExtrasKey);
        if (extras == null)
            return null;
        
        var data = Deserialize(extras);
        return new CrossBundle(data);
    }

    public void Write(Bundle bundle, ICrossBundle? savedState)
    {
        if (savedState == null)
            return;

        if (savedState.Data.Count == 0)
            return;

        var data = Serialize(savedState.Data);
        bundle.PutByteArray(ExtrasKey, data);
    }

    private byte[] Serialize(IDictionary<string, string> values)
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);

        writer.Write(values.Count);
        foreach (var value in values)
        {
            writer.Write(value.Key);
            writer.Write(value.Value);
        }

        return ms.ToArray();
    }

    private Dictionary<string, string> Deserialize(byte[] buffer)
    {
        using var ms = new MemoryStream(buffer);
        using var reader = new BinaryReader(ms);

        var count = reader.ReadInt32();
        var disctionary = new Dictionary<string, string>(count);

        for (int i = 0; i < count; i++)
        {
            disctionary.Add(reader.ReadString(), reader.ReadString());
        }

        return disctionary;
    }
}
