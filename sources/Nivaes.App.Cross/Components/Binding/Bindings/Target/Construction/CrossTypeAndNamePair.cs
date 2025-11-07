namespace Nivaes.App.Cross
{
    public class CrossTypeAndNamePair
    {
        public CrossTypeAndNamePair()
        {
        }

        public CrossTypeAndNamePair(Type type, string name)
        {
            Type = type;
            Name = name;
        }

        public Type Type { get; set; }
        public string Name { get; set; }
    }
}
