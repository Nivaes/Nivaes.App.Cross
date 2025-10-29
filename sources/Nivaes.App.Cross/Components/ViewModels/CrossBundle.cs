namespace Nivaes.App.Cross
{
    public class CrossBundle : ICrossBundle
    {
        public CrossBundle()
            : this(new Dictionary<string, string>())
        {
        }

        public CrossBundle(IDictionary<string, string> data)
        {
            Data = data ?? new Dictionary<string, string>();
        }

        public IDictionary<string, string> Data { get; private set; }

        //public void Write(object toStore)
        //{
        //    Data.Write(toStore);
        //}

        //public T Read<T>()
        //    where T : new()
        //{
        //    return Data.Read<T>();
        //}

        //public object Read(Type type)
        //{
        //    return Data.Read(type);
        //}
    }
}
