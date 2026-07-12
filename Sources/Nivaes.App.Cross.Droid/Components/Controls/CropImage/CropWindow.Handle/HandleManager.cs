namespace Nivaes.App.Cross.Droid.GropImage
{
    internal class HandleManager
    {
        public static Handle
            TopLeft, TopRight, BottomLeft, BottomRight, Left, Top, Right, Bottom, Center;

        static HandleManager()
        {
            TopLeft = new Handle(new CornerHandleHelper(EdgeManager.Top, EdgeManager.Left), HandleType.TopLeft);
            TopRight = new Handle(new CornerHandleHelper(EdgeManager.Top, EdgeManager.Right), HandleType.TopRight);
            BottomLeft = new Handle(new CornerHandleHelper(EdgeManager.Bottom, EdgeManager.Left),
                HandleType.BottomLeft);
            BottomRight = new Handle(new CornerHandleHelper(EdgeManager.Bottom, EdgeManager.Right),
                HandleType.BottomRight);
            Left = new Handle(new VerticalHandleHelper(EdgeManager.Left), HandleType.Left);
            Top = new Handle(new HorizontalHandleHelper(EdgeManager.Top), HandleType.Top);
            Right = new Handle(new VerticalHandleHelper(EdgeManager.Right), HandleType.Right);
            Bottom = new Handle(new HorizontalHandleHelper(EdgeManager.Bottom), HandleType.Bottom);
            Center = new Handle(new CenterHandleHelper(), HandleType.Center);
        }
    }
}
