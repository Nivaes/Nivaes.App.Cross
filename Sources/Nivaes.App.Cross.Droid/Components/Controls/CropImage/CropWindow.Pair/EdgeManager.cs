namespace Nivaes.App.Cross.Droid.GropImage
{
    public static class EdgeManager
    {
        public static Edge Left, Top, Right, Bottom;

        static EdgeManager()
        {
            Left = new Edge(EdgeType.Left);
            Top = new Edge(EdgeType.Top);
            Right = new Edge(EdgeType.Right);
            Bottom = new Edge(EdgeType.Bottom);
        }
    }
}
