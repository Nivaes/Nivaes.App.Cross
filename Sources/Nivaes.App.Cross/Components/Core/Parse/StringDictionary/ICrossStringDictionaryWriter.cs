namespace Nivaes.App.Cross
{
    [Obsolete("", true)]
    public interface ICrossStringDictionaryWriter
    {
        string Write(IDictionary<string, string>? dictionary);
    }
}
