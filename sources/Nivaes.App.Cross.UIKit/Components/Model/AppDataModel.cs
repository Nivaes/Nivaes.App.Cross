namespace Nivaes.App.Cross.UIKit
{
    using System.Security.Principal;
    using Nivaes.App.Cross.UIKit.Presenters;

    public sealed class AppDataModel
    {
        public UIWindow Windows { get; }

        public AppDataModel(UIWindow windows)
        {
            Windows = windows;
        }
    }
}
