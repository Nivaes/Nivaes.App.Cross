namespace MvvmCross.Plugin.Json
{
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json;
    using Nivaes.App.Cross;

    [Preserve(AllMembers = true)]
    [RequiresUnreferencedCode("Uses JsonSerializer which may not be fully preserved in trimming scenarios")]
    public class MvxJsonConverter
        : ICrossJsonConverter
    {
        public JsonSerializerOptions Settings { get; set; }

        public MvxJsonConverter()
        {
            Settings = new JsonSerializerOptions
            {
                WriteIndented = false
            };
        }

        public T? DeserializeObject<T>(string inputText)
        {
            return JsonSerializer.Deserialize<T>(inputText, Settings);
        }

        public object? DeserializeObject(Type type, string inputText)
        {
            return JsonSerializer.Deserialize(inputText, type, Settings);
        }

        public T? DeserializeObject<T>(Stream stream)
        {
            return JsonSerializer.Deserialize<T>(stream, Settings);
        }

        public string SerializeObject(object toSerialise)
        {
            return JsonSerializer.Serialize(toSerialise, Settings);
        }
    }
}
