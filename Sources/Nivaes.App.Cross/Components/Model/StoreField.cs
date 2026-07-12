namespace Nivaes.App.Cross
{
    using System;

    public class StoreField
    {
        public string Container { get; private set; }
        public Guid Id { get; private set; }
        public string Field { get; private set; }

        public StoreField(string container, Guid id, string field)
        {
            Container = container;
            Id = id;
            Field = field;
        }
    }
}
