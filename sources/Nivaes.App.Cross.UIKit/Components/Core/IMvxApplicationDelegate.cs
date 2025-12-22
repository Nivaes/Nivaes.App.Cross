namespace MvvmCross.Platforms.Ios.Core
{
    using Nivaes.App.Cross;

    public interface IMvxApplicationDelegate 
        : IUIApplicationDelegate, ICrossLifetime;
}