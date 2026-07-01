namespace Nivaes.App.Cross
{
    public interface ICrossStringDictionaryWriter
    {
        string Write(IDictionary<string, string>? dictionary);
    }
}
