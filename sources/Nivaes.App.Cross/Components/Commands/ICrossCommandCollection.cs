namespace Nivaes.App.Cross
{
    public interface ICrossCommandCollection
    {
        ICrossCommand? this[string name] { get; }
    }
}
