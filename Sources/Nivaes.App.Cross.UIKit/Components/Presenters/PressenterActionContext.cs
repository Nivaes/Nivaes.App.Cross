namespace Nivaes.App.Cross.UIKitLib
{
    public sealed class PressenterActionContext
    {
        public UIWindow Window { get; }

        public PressenterActionContext(UIWindow window)
        {
            Window = window;
        }
    }
}
