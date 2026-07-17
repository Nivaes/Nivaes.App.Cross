namespace Nivaes.App.Cross.UIKitLib
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class MenuPresentationAttribute
        : BasePresentationAttribute
    {
        public MenuPanelPosition MenuPosition { get; set; } = MenuPanelPosition.Left;

        public MenuPresentationAttribute()
        {
        }

        public MenuPresentationAttribute(MenuPanelPosition menuPosition)
        {
            MenuPosition = menuPosition;
        }
    }
}
