namespace Nivaes.App.Cross.Droid.GropImage
{
    public class EdgePair
    {
        public Edge Primary { get; set; }
        public Edge Secondary { get; set; }

        public EdgePair(Edge primary, Edge secondary)
        {
            Primary = primary;
            Secondary = secondary;
        }
    }
}
