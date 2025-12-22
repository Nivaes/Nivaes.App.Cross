namespace MvvmCross.Platforms.Mac.Core
{
    using AppKit;
    using Nivaes.App.Cross;

    public interface IMvxApplicationDelegate 
        : INSApplicationDelegate, ICrossLifetime
    {
    }
}
