namespace Nivaes.App.Cross.UIKit
{
    public interface ICrossTabBarItemViewController
    {
        string TabName { get; }
        string TabIconName { get; }

        string TabSelectedIconName { get; }
    }
}
