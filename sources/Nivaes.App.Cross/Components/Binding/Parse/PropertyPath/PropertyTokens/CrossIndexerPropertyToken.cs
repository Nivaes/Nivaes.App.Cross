namespace Nivaes.App.Cross
{

    public record CrossIndexerPropertyToken(object? Key) : ICrossPropertyToken
    {
        public override string ToString()
        {
            return "IndexedProperty:" + (Key == null ? "null" : Key.ToString());
        }
    }

    public record MvxIndexerPropertyToken<T>(T? Key) : CrossIndexerPropertyToken(Key)
    {
        public new T? Key => (T?)base.Key;
    }
}