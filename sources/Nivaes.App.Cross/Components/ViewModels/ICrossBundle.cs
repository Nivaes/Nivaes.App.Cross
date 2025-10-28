namespace Nivaes.App.Cross
{
    public interface ICrossBundle
    {
        IDictionary<string, string> Data { get; }

        //void Write(object toStore);

        //T Read<T>() where T : new();

        //object Read(Type type);
    }
}
