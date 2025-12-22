namespace MvvmCross.Platforms.Tvos.Core
{
    using Nivaes.App.Cross;
    using UIKit;

    public interface IMvxApplicationDelegate 
        : IUIApplicationDelegate, ICrossLifetime
    {
    }
}
