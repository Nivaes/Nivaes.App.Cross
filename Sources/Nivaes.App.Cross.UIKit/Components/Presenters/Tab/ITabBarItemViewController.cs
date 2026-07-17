namespace Nivaes.App.Cross.UIKitLib;

public interface ITabBarItemViewController
{
    string TabName { get; }
    string TabIconName { get; }

    string TabSelectedIconName { get; }
}
