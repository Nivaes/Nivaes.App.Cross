using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross;

public class CrossViewModelRequestCustomTextSerializer
    : ICrossTextSerializer
{

    private readonly Lazy<CrpssStringDictionaryWriter> _stringDictionaryWriter =
        new(() => new CrpssStringDictionaryWriter());

    private readonly Lazy<CrossStringDictionaryParser> _stringDictionaryParser =
        new(() => new CrossStringDictionaryParser());

    public string SerializeObject(object toSerialise)
    {
        if (toSerialise is CrossViewModelRequest viewModelRequest)
            return Serialize(viewModelRequest);

        if (toSerialise is IDictionary<string, string> stringDictionary)
            return Serialize(stringDictionary);

        throw new AppException("This serializer only knows about MvxViewModelRequest and IDictionary<string,string>");
    }

    [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
    public T DeserializeObject<T>(string inputText)
    {
        return (T)DeserializeObject(typeof(T), inputText);
    }

    [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
    public object DeserializeObject(Type type, string inputText)
    {
        if (type == typeof(CrossViewModelRequest))
            return DeserializeViewModelRequest(inputText);

        if (typeof(IDictionary<string, string>).IsAssignableFrom(type))
            return DeserializeStringDictionary(inputText);

        throw new AppException("This serializer only knows about MvxViewModelRequest and IDictionary<string,string>");
    }

    protected virtual IDictionary<string, string> DeserializeStringDictionary(string inputText)
    {
        var dictionary = _stringDictionaryParser.Value.Parse(inputText);
        return dictionary;
    }

    protected virtual CrossViewModelRequest DeserializeViewModelRequest(string inputText)
    {
        var dictionary = _stringDictionaryParser.Value.Parse(inputText);
        var toReturn = new CrossViewModelRequest();
        var viewModelTypeName = SafeGetValue(dictionary, "Type");
        toReturn.ViewModelType = DeserializeViewModelType(viewModelTypeName);
        toReturn.ParameterValues = _stringDictionaryParser.Value.Parse(SafeGetValue(dictionary, "Params"));
        toReturn.PresentationValues = _stringDictionaryParser.Value.Parse(SafeGetValue(dictionary, "Pres"));
        return toReturn;
    }

    protected virtual string Serialize(IDictionary<string, string> toSerialise)
    {
        return _stringDictionaryWriter.Value.Write(toSerialise);
    }

    protected virtual string Serialize(CrossViewModelRequest toSerialise)
    {
        ArgumentNullException.ThrowIfNull(toSerialise, nameof(toSerialise));

        var dictionary = new Dictionary<string, string>
        {
            ["Type"] = SerializeViewModelName(toSerialise.ViewModelType),
            ["Params"] = _stringDictionaryWriter.Value.Write(toSerialise.ParameterValues),
            ["Pres"] = _stringDictionaryWriter.Value.Write(toSerialise.PresentationValues)
        };
        return _stringDictionaryWriter.Value.Write(dictionary);
    }

    protected virtual string SerializeViewModelName(Type? viewModelType)
    {
        ArgumentNullException.ThrowIfNull(viewModelType?.FullName, nameof(viewModelType));

        return viewModelType.FullName;
    }

    protected virtual Type? DeserializeViewModelType(string viewModelTypeName)
    {
        if (!Singleton<NameViewModelsKeyContainerManager>.Instance.TryGetValue(viewModelTypeName, out var toReturn))
        {
            throw new AppException("Failed to find viewmodel for {0}", viewModelTypeName);
        }

        return toReturn;
    }

    private static string SafeGetValue(IDictionary<string, string> dictionary, string key)
    {
        if (!dictionary.TryGetValue(key, out var value))
            throw new AppException("Dictionary missing required key/value pair for key {0}", key);
        return value;
    }
}