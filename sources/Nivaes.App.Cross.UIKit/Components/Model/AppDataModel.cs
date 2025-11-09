namespace Nivaes.App.Cross.UIKit
{
    public sealed class AppDataModel
    {
        public UIWindow Windows { get; }

        public AppDataModel(UIWindow windows)
        {
            Windows = windows;
        }
    }
}
